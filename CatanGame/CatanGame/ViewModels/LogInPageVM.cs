using CatanGame.Models;
using CatanGame.ModelsLogic;
using System.Windows.Input;
using CatanGame.Views;

namespace CatanGame.ViewModels
{
    public partial class LogInPageVM : ObservableObject
    {
        private readonly  User user = new();
        public ICommand LoginCommand { get; }
        public ICommand CreateAcoountPageCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand PasswordReset { get; }
        public bool IsBusy { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisibleEmailMessege { get; set; } = true;
        public bool IsVisiblePasswordMessege { get; set; } = false;
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

        public LogInPageVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            CreateAcoountPageCommand = new Command(GoToRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            PasswordReset = new Command(GoToResetPassword);
            user.OnAuthFalier += OnAuthFalier;
            user.OnAuthComplete += OnAuthComplete;
        }

        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            }
            IsBusy = false;
            IsEnabled = true;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
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
                IsBusy = false;
                IsEnabled = true;
                Password = string.Empty;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(IsEnabled));
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

        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !(user.Email.Contains('@') && user.Email.Contains('.'));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }

        private void ToggleIsVisiblePasswordMessege()
        {
            IsVisiblePasswordMessege = (user.Email.Contains('@') && user.Email.Contains('.')) && !(user.Password.Length >= 8 && user.Password.Length <= 12);
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
        }

        private void Login()
        {
            IsBusy = true;
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
            user.Login();
        }

        private bool CanLogin()
        {
            return (Email.Contains('@') && Email.Contains('.') && Password.Length >= 8 && Password.Length <= 12);
        }

        private void GoToResetPassword()
        {
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            if (Application.Current != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new PassWordResetPage();
                    IsEnabled = true;
                    OnPropertyChanged(nameof(IsEnabled));
                });
            }
        }

        public void GoToRegister()
        {
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            if (Application.Current != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new RegisterPage();
                    IsEnabled = true;
                    OnPropertyChanged(nameof(IsEnabled));
                });
            }
        }
    }
}
