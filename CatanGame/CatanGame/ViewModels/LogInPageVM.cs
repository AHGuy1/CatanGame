using CatanGame.Models;
using System;
using System.Windows.Input;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    public partial class LogInPageVM : ObservableObject
    {
        private readonly  User user = new();
        public ICommand LoginCommand { get; }
        public ICommand CreateAcoountPage { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public bool IsBusy { get; set; } = false;
        public bool IsVisibleUserNameMessege { get; set; } = true;
        public bool IsVisiblePasswordMessege { get; set; } = false;
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
        }

        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        private void ToggleIsVisibleUserNameMessege()
        {
            IsVisibleUserNameMessege = string.IsNullOrWhiteSpace(user.UserName);
            OnPropertyChanged(nameof(IsVisibleUserNameMessege));
        }

        private void ToggleIsVisiblePasswordMessege()
        {
            IsVisiblePasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && string.IsNullOrWhiteSpace(user.Password);
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
        }

        private void Login()
        {
            user.Login();
        }

        private bool CanLogin()
        {
            return (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password));
        }
        public static void GoToRegister()
        {

        }
    }
}
