using CatanGame.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CatanGame.ModelsLogic
{
    public class User : UserModel
    {
        #region Constructor
        public User()
        {
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
        }
        #endregion

        #region Private Methods
        // Handles the result of a registration request.
        protected override void RegisterOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.AcoountCreated, ToastDuration.Long, 20).Show());
                AuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(FbData.GetErrorMessage(msg), ToastDuration.Long, 20).Show());
                AuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.UnknownError, ToastDuration.Long, 20).Show());
                AuthFalier?.Invoke(this, EventArgs.Empty);
            }
        }

        // Handles the result of a password reset request.
        protected override void ResetPasswordOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                AuthComplete?.Invoke(this, EventArgs.Empty);
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(FbData.GetErrorMessage(msg), ToastDuration.Long, 20).Show());
                AuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.UnknownError, ToastDuration.Long, 20).Show());
                AuthFalier?.Invoke(this, EventArgs.Empty);
            }
        }

        // Handles the result of a login request.
        protected override void LoginOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.LoginSuccessMessage, ToastDuration.Short, 20).Show());
                AuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(FbData.GetErrorMessage(msg), ToastDuration.Long, 20).Show());
                AuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.UnknownError, ToastDuration.Long, 20).Show());
                AuthFalier?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Public Methods
        // Starts account registration with the current user details.
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, RegisterOnComplete);
        }

        // Starts login with the current credentials.
        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, LoginOnComplete);
        }

        // Starts logout for the current user.
        public override void LogOut()
        {
            fbd.SignOut();
        }

        // Starts a password reset for the current email.
        public override void ResetPassword()
        {
            fbd.ResetPassword(Email, ResetPasswordOnComplete);
        }

        // Saves or clears remembered login credentials.
        public override void RememberMe()
        {
            if (Preferences.Get(Keys.IsRememberedKey, false))
            {
                Preferences.Set(Keys.EmailKey, Email);
                Preferences.Set(Keys.PasswordKey, Password);
                Preferences.Set(Keys.IsRememberedKey, true);
            }
            else
            {
                Preferences.Remove(Keys.EmailKey);
                Preferences.Remove(Keys.PasswordKey);
                Preferences.Set(Keys.IsRememberedKey, false);
            }
        }
        #endregion
    }
}
