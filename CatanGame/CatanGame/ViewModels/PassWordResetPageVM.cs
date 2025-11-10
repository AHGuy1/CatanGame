using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class PassWordResetPageVM : ObservableObject
    {
        private readonly User user = new();
        public ICommand ResetPassWordCommand { get; }
        public ICommand SwitchPageBackCommand { get; }
        public ICommand SwitchToLogInPageCommand { get; }
        public bool IsVisibleEmailMessege { get; set; } = true;
        public bool IsVisibleBeforePassWordReset { get; set; } = true;
        public bool IsBusy { get; set; } = false;
        public bool IsVisibleAfterPassWordReset { get; set; } = false;

        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (ResetPassWordCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleEmailMessege();
            }
        }

        public PassWordResetPageVM()
        {
            ResetPassWordCommand = new Command(ResetPassWord, CanResetPassWord);
            SwitchPageBackCommand = new Command(ChangePage);
            SwitchToLogInPageCommand = new Command(SwitchToLogInPage);
            user.OnAuthComplete += OnAuthComplete;
            user.OnAuthFalier += OnAuthFalier;
        }

        private void OnAuthComplete(object? sender, EventArgs e)
        {
            ChangePage();
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
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(Email));
            });
        }

        private void ChangePage()
        {
            IsVisibleAfterPassWordReset = !IsVisibleAfterPassWordReset;
            IsVisibleBeforePassWordReset = !IsVisibleBeforePassWordReset;
            IsBusy = false;
            OnPropertyChanged(nameof(IsVisibleBeforePassWordReset));
            OnPropertyChanged(nameof(IsVisibleAfterPassWordReset));
            OnPropertyChanged(nameof(IsBusy));
        }

        private void SwitchToLogInPage()
        {
            if (Application.Current != null)
            {              
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new LogInPage();
                });
            }
        }

        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !(user.Email.Contains('@') && user.Email.Contains('.'));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }
        private bool CanResetPassWord()
        {
            return user.Email.Contains('@') && user.Email.Contains('.');
        }
        private void ResetPassWord()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));
            user.ResetPassword();
        }
    }
    
}
