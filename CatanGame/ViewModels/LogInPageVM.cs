using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class LogInPageVM : ObservableObject
    {
        private readonly  User user = new();
        public ICommand LoginCommand { get; }
        public ICommand LoginWithVerificationCodeCommand { get; }
        public ICommand SendVerificationCodeToPhoneCommand { get; }
        public ICommand CreateAcoountPageCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand PasswordReset { get; }
        public bool IsBusy { get; set; } = false;
        public bool IsVisibileBeforeVerificationCodeSent { get; set; } = true;
        public bool IsVisibileAfterVerificationCodeSent { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisibleEmailMessege { get; set; } = true;
        public bool IsVisiblePhoneMessege { get; set; } = false;
        public bool IsVisiblePasswordMessege { get; set; } = false;
        public bool IsVisibleVerificationCodeMessege { get; set; } = true;
        public bool IsPassword { get; set; } = true;
        public bool IsRemembered
        {
            get => Preferences.Get(Keys.IsRememberedKey, false);
            set
            {
                Preferences.Set(Keys.IsRememberedKey, value);
                RememberMe();
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                RememberMe();
                ToggleIsVisibleEmailMessege();
                ToggleIsVisiblePasswordMessege();
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                RememberMe();
                ToggleIsVisibleEmailMessege();
                ToggleIsVisiblePasswordMessege();
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public string VerificationCode
        {
            get => VerificationCode;
            set
            {

                ToggleIsVisibleVerificationCodeMessege();
                (LoginWithVerificationCodeCommand as Command)?.ChangeCanExecute();
            }
        }
        public string PhoneNumber
        {
            get => PhoneNumber;
            set
            {
                ToggleIsVisiblePhoneMessege();
                (SendVerificationCodeToPhoneCommand as Command)?.ChangeCanExecute();
            }
        }

        public LogInPageVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            LoginWithVerificationCodeCommand = new Command(LoginWithVerificationCode, CanLoginWithVerificationCode);
            SendVerificationCodeToPhoneCommand = new Command(SendVerificationCodeToPhone, CanSendVerificationCodeToPhone);
            CreateAcoountPageCommand = new Command(GoToRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            PasswordReset = new Command(GoToResetPassword);
            user.AuthFalier += OnAuthFalier;
            user.AuthComplete += OnAuthComplete;
            user.PhoneNumberFalier += OnPhoneContactFalier;
            user.PhoneNumberComplete += OnPhoneContactComplete;
        }

        private void NotBusy()
        {
            IsBusy = false;
            IsEnabled = true;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
        }
        private void Busy()
        {
            IsBusy = true;
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
        }
        private void OnPhoneContactComplete(object? sender, EventArgs e)
        {
            IsVisibileBeforeVerificationCodeSent = false;
            IsVisibileAfterVerificationCodeSent = true;
            NotBusy();
            OnPropertyChanged(nameof(IsVisibileBeforeVerificationCodeSent));
            OnPropertyChanged(nameof(IsVisibileAfterVerificationCodeSent));
        }
        private void OnPhoneContactFalier(object? sender, EventArgs e)
        {
            PhoneNumber = Strings.PhoneAreaCode;
        }
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
           NotBusy();
        }
        private void OnAuthFalier(object? sender, EventArgs e)
        {
            ResetFields();
        }
        private void ResetFields()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Email = string.Empty;
                Password = string.Empty;
                NotBusy();
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Password));
            });
        }
        private void RememberMe()
        {
            user.RememberMe();
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        private void ToggleIsVisiblePhoneMessege()
        {
            IsVisiblePhoneMessege = !String.Equals(PhoneNumber, Strings.PhoneStructure);
            OnPropertyChanged(nameof(IsVisiblePhoneMessege));
        }
        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !(user.Email.Contains(Strings.AtSign) && user.Email.Contains(Strings.Dot));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }
        private void ToggleIsVisibleVerificationCodeMessege()
        {
            IsVisibleVerificationCodeMessege = !String.IsNullOrWhiteSpace(VerificationCode);
            OnPropertyChanged(nameof(IsVisibleVerificationCodeMessege));
        }
        private void ToggleIsVisiblePasswordMessege()
        {
            IsVisiblePasswordMessege = (user.Email.Contains(Strings.AtSign) && user.Email.Contains(Strings.Dot)) && !(user.Password.Length >= 8 && user.Password.Length <= 12);
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
        }
        private void Login()
        {
            Busy();
            user.Login();
        }
        private bool CanLogin()
        {
            return (Email.Contains(Strings.AtSign) && Email.Contains(Strings.Dot) && Password.Length >= 8 && Password.Length <= 12);
        }
        private void LoginWithVerificationCode()
        {
            Busy();
            user.SignInWithPhoneNumber(VerificationCode);
        }
        private bool CanLoginWithVerificationCode()
        {
            return !String.IsNullOrWhiteSpace(VerificationCode);
        }
        private void SendVerificationCodeToPhone()
        {

            Busy();
            user.VerifyPhoneNumber(PhoneNumber);
        }
        private bool CanSendVerificationCodeToPhone()
        {
            return String.Equals(PhoneNumber,Strings.PhoneStructure);
        }
        private void GoToResetPassword()
        {
            Busy();
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new PassWordResetPage();
                    IsEnabled = true;
                    OnPropertyChanged(nameof(IsEnabled));
                });
        }
        private void GoToRegister()
        {
            Busy();
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new RegisterPage();
                    IsEnabled = true;
                    OnPropertyChanged(nameof(IsEnabled));
                });
        }
    }
}
