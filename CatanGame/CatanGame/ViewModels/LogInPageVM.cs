using CatanGame.Models;
using System;
using System.Windows.Input;
using CatanGame.ModelsLogic;
using System.Threading.Tasks;

namespace CatanGame.ViewModels
{
    public partial class LogInPageVM : ObservableObject
    {
        private readonly  User user = new();
        public ICommand LoginCommand { get; }
        public ICommand CreateAcoountPage { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand PasswordReset { get; }
        public bool IsBusy { get; set; } = false;
        public bool IsVisibleEmailMessege { get; set; } = true;
        public bool IsVisiblePasswordMessege { get; set; } = false;
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (LoginCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleEmailMessege();
                ToggleIsVisiblePasswordMessege();
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleEmailMessege();
                ToggleIsVisiblePasswordMessege();
            }
        }
        public bool IsPassword { get; set; } = true;

        public LogInPageVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            CreateAcoountPage = new Command(GoToRegister);
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
        }

        private void OnAuthFalier(object? sender, EventArgs e)
        {
            Email = "";
            Password = "";
            OnPropertyChanged(Password);
            OnPropertyChanged(Email);
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
            user.Login();
        }

        private bool CanLogin()
        {
            return (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password));
        }

        private void GoToResetPassword()
        {

        }

        public static void GoToRegister()
        {

        }
    }
}
