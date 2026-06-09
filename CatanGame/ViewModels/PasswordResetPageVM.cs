using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class PasswordResetPageVM : ObservableObject
    {
        #region Fields
        private readonly User user = new();
        private readonly ModelsLogic.Connectivity connectivity = new();
        #endregion

        #region Commands
        public ICommand ResetPassWordCommand { get; }
        public ICommand SwitchPageBackCommand { get; }
        public ICommand SwitchToLogInPageCommand { get; }
        #endregion

        #region Properties
        public bool IsDisconnected => !connectivity.IsConnected;
        public bool IsVisibleEmailMessege { get; set; } = true;
        public bool IsVisibleBeforePassWordReset { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
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
        #endregion

        #region Constructor
        public PasswordResetPageVM()
        {
            ResetPassWordCommand = new Command(ResetPassWord, CanResetPassWord);
            SwitchPageBackCommand = new Command(ChangePage);
            SwitchToLogInPageCommand = new Command(SwitchToLogInPage);
            connectivity.ConnectivityChanged += OnConnectivityChanged;
            user.AuthComplete += OnAuthComplete;
            user.AuthFalier += OnAuthFalier;
        }
        #endregion

        #region Private Methods
        //Updates xaml that connectivity status has changed.
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            IsEnabled = connectivity.IsConnected;
            OnPropertyChanged(nameof(IsDisconnected));
            OnPropertyChanged(nameof(IsEnabled));
        }

        // Shows the reset confirmation state.
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            ChangePage();
        }

        // Resets the form after reset failure.
        private void OnAuthFalier(object? sender, EventArgs e)
        {
            ResetFields();
        }

        // Clears the email field and busy state.
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

        // Switches between reset form and confirmation views.
        private void ChangePage()
        {
            IsVisibleAfterPassWordReset = !IsVisibleAfterPassWordReset;
            IsVisibleBeforePassWordReset = !IsVisibleBeforePassWordReset;
            IsBusy = false;
            OnPropertyChanged(nameof(IsVisibleBeforePassWordReset));
            OnPropertyChanged(nameof(IsVisibleAfterPassWordReset));
            OnPropertyChanged(nameof(IsBusy));
        }

        // Navigates back to the login page.
        private void SwitchToLogInPage()
        {
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new LogInPage();
                });
        }

        // Updates email validation message visibility.
        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !(user.Email.Contains('@') && user.Email.Contains('.'));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }

        // Checks whether password reset can be requested.
        private bool CanResetPassWord()
        {
            return user.Email.Contains('@') && user.Email.Contains('.');
        }

        // Starts the password reset request.
        private void ResetPassWord()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));
            user.ResetPassword();
        }
        #endregion
    }
    
}
