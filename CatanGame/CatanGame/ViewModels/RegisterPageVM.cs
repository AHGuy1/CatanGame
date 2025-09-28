using CatanGame.Models;
using System;
using System.Windows.Input;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    internal class RegisterPageVM : ObservableObject
    {
        private User user = new();
        public ICommand RegisterCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand ToggleIsPasswordCommandConfirmPassword { get; }
        public ICommand VisibleUserNameMessegeCommand { get; }
        public ICommand VisiblePasswordMessegeCommand { get; }
        public ICommand VisibleConfirmPasswordMessegeCommand { get; }
        public ICommand VisibleEmailMessegeCommand { get; }
        public bool IsPasswordConfirmPassword { get; set; } = true;
        public bool IsPassword { get; set; } = true;
        public bool IsVisibleUserNameMessege { get; set; } = true;
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
                (VisibleUserNameMessegeCommand as Command)?.ChangeCanExecute();
                (VisiblePasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleConfirmPasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleEmailMessegeCommand as Command)?.ChangeCanExecute();

            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                (VisibleUserNameMessegeCommand as Command)?.ChangeCanExecute();
                (VisiblePasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleConfirmPasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleEmailMessegeCommand as Command)?.ChangeCanExecute();
            }
        }
        public string ConfirmPassword
        {
            get => user.ConfirmPassword;
            set
            {
                user.ConfirmPassword = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                (VisibleUserNameMessegeCommand as Command)?.ChangeCanExecute();
                (VisiblePasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleConfirmPasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleEmailMessegeCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
                (VisibleUserNameMessegeCommand as Command)?.ChangeCanExecute();
                (VisiblePasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleConfirmPasswordMessegeCommand as Command)?.ChangeCanExecute();
                (VisibleEmailMessegeCommand as Command)?.ChangeCanExecute();
            }
        }

        public RegisterPageVM()
        {
            RegisterCommand = new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            ToggleIsPasswordCommandConfirmPassword = new Command(ToggleIsPasswordConfirmPassword);
            VisibleUserNameMessegeCommand = new Command(ToggleIsVisibleUserNameMessege);
            VisiblePasswordMessegeCommand = new Command(ToggleIsVisiblePasswordMessege);
            VisibleConfirmPasswordMessegeCommand = new Command(ToggleIsVisibleConfirmPasswordMessege);
            VisibleEmailMessegeCommand = new Command(ToggleIsVisibleEmailMessege);
        }

        public bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.ConfirmPassword) && !string.IsNullOrWhiteSpace(user.Email) && user.Password == user.ConfirmPassword && user.Email.Contains('@') && user.Email.Contains('.'));
        }

        private void Register()
        {
            user.Register();
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

        private void ToggleIsVisibleConfirmPasswordMessege()
        {
            IsVisibleConfirmPasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && user.Password != user.ConfirmPassword;
            OnPropertyChanged(nameof(IsVisibleConfirmPasswordMessege));
        }

        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && user.Password == user.ConfirmPassword && !(user.Email.Contains('@') && user.Email.Contains('.')));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }

        private void ToggleIsPasswordConfirmPassword()
        {
            IsPasswordConfirmPassword = !IsPasswordConfirmPassword;
            OnPropertyChanged(nameof(IsPasswordConfirmPassword));
        }

    }
}
