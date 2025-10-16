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
        public bool IsVisibleUserNameMessege { get; set; } = true;
        public bool IsVisiblePasswordMessege { get; set; } = false;
        public bool IsVisiblePasswordOrUserNameMessege { get; set; } = false;
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LoginCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleUserNameMessege();
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
                ToggleIsVisibleUserNameMessege();
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
        }

        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        private void ToggleIsVisibleUserNameMessege()
        {
            IsVisibleUserNameMessege = string.IsNullOrWhiteSpace(user.UserName);
            IsVisiblePasswordOrUserNameMessege = false;
            OnPropertyChanged(nameof(IsVisibleUserNameMessege));
            OnPropertyChanged(nameof(IsVisiblePasswordOrUserNameMessege));
        }

        private void ToggleIsVisiblePasswordMessege()
        {
            IsVisiblePasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && !(user.Password.Length >= 8 && user.Password.Length <= 12);
            IsVisiblePasswordOrUserNameMessege = false;
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
            OnPropertyChanged(nameof(IsVisiblePasswordOrUserNameMessege));
        }

        private void ToggleIsVisiblePasswordOrUserNameMessege()
        {
            IsVisiblePasswordOrUserNameMessege = true;
            OnPropertyChanged(nameof(IsVisiblePasswordOrUserNameMessege));
            user.InvalidEmailOrPassword = false;
        }

        private void Login()
        {
            user.Login();
            if(user.InvalidEmailOrPassword) ToggleIsVisiblePasswordOrUserNameMessege();
        }

        private bool CanLogin()
        {
            return (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password));
        }

        private void GoToResetPassword()
        {

        }

        public static void GoToRegister()
        {

        }
    }
}
