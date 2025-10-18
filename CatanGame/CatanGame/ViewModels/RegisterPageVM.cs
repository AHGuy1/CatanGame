using CatanGame.Models;
using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class RegisterPageVM : ObservableObject
    {
        private readonly User user = new();
        public ICommand RegisterCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand ToggleIsPasswordCommandConfirmPassword { get; }
        public bool IsPasswordConfirmPassword { get; set; } = true;
        public bool IsPassword { get; set; } = true;
        public bool IsVisibleUserNameMessege { get; set; } = true;
        public bool IsBusy { get; set; } = false;
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

        public RegisterPageVM()
        {
            RegisterCommand = new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            ToggleIsPasswordCommandConfirmPassword = new Command(ToggleIsPasswordConfirmPassword);
            user.OnAuthComplete += OnAuthComplete;
            user.OnAuthFalier += OnAuthFalier;
        }

        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if(Application.Current != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            }
            IsBusy = false;
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
                UserName = string.Empty;
                ConfirmPassword = string.Empty;
                IsBusy = false;
                Password = string.Empty;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(ConfirmPassword));
                OnPropertyChanged(nameof(UserName));
            });
        }

        public bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.ConfirmPassword) && !string.IsNullOrWhiteSpace(user.Email) && user.Password == user.ConfirmPassword && user.Email.Contains('@') && user.Email.Contains('.'));
        }

        private void Register()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));
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
            IsVisiblePasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8 || user.Password.Length > 12);
            OnPropertyChanged(nameof(IsVisiblePasswordMessege));
        }

        private void ToggleIsVisibleConfirmPasswordMessege()
        {
            IsVisibleConfirmPasswordMessege = !string.IsNullOrWhiteSpace(user.UserName) && !(string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8 || user.Password.Length > 12) && user.Password != user.ConfirmPassword;
            OnPropertyChanged(nameof(IsVisibleConfirmPasswordMessege));
        }

        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !string.IsNullOrWhiteSpace(user.UserName) && !(string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8 || user.Password.Length > 12) && user.Password == user.ConfirmPassword && !(user.Email.Contains('@') && user.Email.Contains('.'));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }

        private void ToggleIsPasswordConfirmPassword()
        {
            IsPasswordConfirmPassword = !IsPasswordConfirmPassword;
            OnPropertyChanged(nameof(IsPasswordConfirmPassword));
        }
       

    }
}
