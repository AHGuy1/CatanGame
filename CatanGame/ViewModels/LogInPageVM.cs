using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class LogInPageVM : ObservableObject
    {
        #region Fields
        private readonly User user = new();
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand CreateAcoountPageCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand PasswordReset { get; }
        #endregion

        #region Properties
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
        #endregion

        #region Constructor
        public LogInPageVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            CreateAcoountPageCommand = new Command(GoToRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            PasswordReset = new Command(GoToResetPassword);
            user.AuthFalier += OnAuthFalier;
            user.AuthComplete += OnAuthComplete;
        }
        #endregion

        #region Private Methods
        // Restores the login form to an interactive state.
        private void NotBusy()
        {
            IsBusy = false;
            IsEnabled = true;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
        }

        // Disables the login form while work is running.
        private void Busy()
        {
            IsBusy = true;
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
        }

        // Navigates to the app shell after successful login.
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            NotBusy();
        }

        // Resets the form after failed login.
        private void OnAuthFalier(object? sender, EventArgs e)
        {
            ResetFields();
        }

        // Clears login fields and busy state.
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

        // Saves or clears remembered credentials.
        private void RememberMe()
        {
            user.RememberMe();
        }

        // Toggles password visibility.
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        // Updates email validation message visibility.
        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !(user.Email.Contains(Strings.AtSign) && user.Email.Contains(Strings.Dot));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }

        // Updates password validation message visibility.
        private void ToggleIsVisiblePasswordMessege()
        {
            IsVisiblePasswordMessege = (user.Email.Contains(Strings.AtSign) && user.Email.Contains(Strings.Dot)) && !(user.Password.Length >= 8 && user.Password.Length <= 12);
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
        }

        // Starts the login request.
        private void Login()
        {
            Busy();
            user.Login();
        }

        // Checks whether the login form is valid.
        private bool CanLogin()
        {
            return (Email.Contains(Strings.AtSign) && Email.Contains(Strings.Dot) && Password.Length >= 8 && Password.Length <= 12);
        }

        // Navigates to the password reset page.
        private void GoToResetPassword()
        {
            Busy();
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new PasswordResetPage();
                    IsEnabled = true;
                    OnPropertyChanged(nameof(IsEnabled));
                });
        }

        // Navigates to the registration page.
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
        #endregion
    }
}
