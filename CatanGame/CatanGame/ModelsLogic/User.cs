using CatanGame.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CatanGame.ModelsLogic
{
    public class User : UserModel
    {
        public User()
        {
            IsRegistered = Preferences.Get(Keys.IsRegisteredKey, false);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
        }

        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, RegisterOnComplete);
        }

        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, LoginOnComplete);
        }

        public override void ResetPassword()
        {
            fbd.ResetPassword(Email, ResetPasswordOnComplete);
        }

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
        private void RegisterOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SaveToPreferences();
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.AcoountCreated, ToastDuration.Long, 20).Show();
                });
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(FbData.GetErrorMessage(msg), ToastDuration.Long, 20).Show();
                });
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.UnknownError, ToastDuration.Long, 20).Show();
                });
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }       
        }
        private void ResetPasswordOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else if(task.Exception != null) 
            {
                string msg = task.Exception.Message;
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(FbData.GetErrorMessage(msg), ToastDuration.Long, 20).Show();
                });
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.UnknownError, ToastDuration.Long, 20).Show();
                });
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
        }

        private void LoginOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.LoginSuccessMessage, ToastDuration.Long, 20).Show();
                });
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
                User.LogedInUserName = fbd.DisplayName;
            }
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(FbData.GetErrorMessage(msg), ToastDuration.Long, 20).Show();
                }); 
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.UnknownError, ToastDuration.Long, 20).Show();
                });
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
        }

        private static void SaveToPreferences()
        {
            Preferences.Set(Keys.IsRegisteredKey, true);
        }
    }
}
