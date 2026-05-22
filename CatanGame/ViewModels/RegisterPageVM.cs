using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class RegisterPageVM : ObservableObject
    {
        #region Fields
        private readonly User user = new();
        #endregion

        #region Commands
        public ICommand RegisterCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand ToggleIsPasswordCommandConfirmPassword { get; }
        #endregion

        #region Properties
        public bool IsPasswordConfirmPassword { get; set; } = true;
        public bool IsPassword { get; set; } = true;
        public bool IsVisibleUserNameMessege { get; set; } = true;
        public bool IsBusy { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisiblePasswordMessege { get; set; } = false;
        public bool IsVisibleConfirmPasswordMessege { get; set; } = false;
        public bool IsVisibleEmailMessege { get; set; } = false;
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleUserNameMessege();
                ToggleIsVisiblePasswordMessege();
                ToggleIsVisibleConfirmPasswordMessege();
                ToggleIsVisibleEmailMessege();
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleUserNameMessege();
                ToggleIsVisiblePasswordMessege();
                ToggleIsVisibleConfirmPasswordMessege();
                ToggleIsVisibleEmailMessege();
            }
        }
        public string ConfirmPassword
        {
            get => user.ConfirmPassword;
            set
            {
                user.ConfirmPassword = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleUserNameMessege();
                ToggleIsVisiblePasswordMessege();
                ToggleIsVisibleConfirmPasswordMessege();
                ToggleIsVisibleEmailMessege();
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleUserNameMessege();
                ToggleIsVisiblePasswordMessege();
                ToggleIsVisibleConfirmPasswordMessege();
                ToggleIsVisibleEmailMessege();
            }
        }
        #endregion

        #region Constructor
        public RegisterPageVM()
        {
            RegisterCommand = new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            ToggleIsPasswordCommandConfirmPassword = new Command(ToggleIsPasswordConfirmPassword);
            user.AuthComplete += OnAuthComplete;
            user.AuthFalier += OnAuthFalier;
        }
        #endregion

        #region Private Methods
        // Returns to login after successful registration.
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new LogInPage();
                });
            IsBusy = false;
            IsEnabled = true;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
        }

        // Resets the form after failed registration.
        private void OnAuthFalier(object? sender, EventArgs e)
        {
            ResetFields();
        }

        // Clears registration fields and busy state.
        private void ResetFields()
        {
            Email = string.Empty;
            UserName = string.Empty;
            ConfirmPassword = string.Empty;
            IsBusy = false;
            IsEnabled = true;
            Password = string.Empty;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(ConfirmPassword));
            OnPropertyChanged(nameof(UserName));
        }

        // Checks whether the registration form is valid.
        private bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.ConfirmPassword) && !string.IsNullOrWhiteSpace(user.Email) && user.Password == user.ConfirmPassword && user.Email.Contains('@') && user.Email.Contains('.'));
        }

        // Starts the registration request.
        private void Register()
        {
            IsBusy = true;
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
            user.Register();
        }

        // Toggles password visibility.
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        // Updates username validation message visibility.
        private void ToggleIsVisibleUserNameMessege()
        {
            IsVisibleUserNameMessege = string.IsNullOrWhiteSpace(user.UserName);
            OnPropertyChanged(nameof(IsVisibleUserNameMessege));
        }

        // Updates password validation message visibility.
        private void ToggleIsVisiblePasswordMessege()
        {
            IsVisiblePasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8 || user.Password.Length > 12);
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
        }

        // Updates confirm password message visibility.
        private void ToggleIsVisibleConfirmPasswordMessege()
        {
            IsVisibleConfirmPasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && !(string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8 || user.Password.Length > 12) && user.Password != user.ConfirmPassword;
            OnPropertyChanged(nameof(IsVisibleConfirmPasswordMessege));
        }

        // Updates email validation message visibility.
        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !string.IsNullOrWhiteSpace(user.UserName) && !(string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8 || user.Password.Length > 12) && user.Password == user.ConfirmPassword && !(user.Email.Contains('@') && user.Email.Contains('.'));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }

        // Toggles confirm password visibility.
        private void ToggleIsPasswordConfirmPassword()
        {
            IsPasswordConfirmPassword = !IsPasswordConfirmPassword;
            OnPropertyChanged(nameof(IsPasswordConfirmPassword));
        }
        #endregion
    }
}
