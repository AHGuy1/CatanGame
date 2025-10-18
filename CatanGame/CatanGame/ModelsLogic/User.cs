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
                    ShowAlert(Strings.AcoountCreated);
                    OnAuthComplete?.Invoke(this, EventArgs.Empty);
                }
                else if (task.Exception != null)
                {
                    string msg = task.Exception.Message;
                    ShowAlert(msg);
                    OnAuthFalier?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    OnAuthFalier?.Invoke(this, EventArgs.Empty);
                    ShowAlert(Strings.UnknownError);
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
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
                ShowAlert(msg);
            }
            else
            {
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
                ShowAlert(Strings.UnknownError);
            }
        }

        private void LoginOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                ShowAlert(Strings.LoginSuccessMessage);
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
            {
                string msg = task.Exception.Message;
                ShowAlert(msg);
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowAlert(Strings.UnknownError);
                OnAuthFalier?.Invoke(this, EventArgs.Empty);
            }
        }

        private static void SaveToPreferences()
        {
            Preferences.Set(Keys.IsRegisteredKey, true);
        }
        public static string ShowAlert(string msg)
        {
            if (msg.Contains(Strings.ContainsINVALID_LOGIN_CREDENTIALS))
                msg = Strings.InvalidCredentialsMessage;
            else if (msg.Contains(Strings.ContainsReason))
            {
                int pos = msg.IndexOf(Strings.ContainsReason);
                msg = msg.Substring((pos + 7), msg.Length - pos - 8);
                for (int i = 1; i < msg.Length; i++)
                {
                    pos = 0;
                    if (char.IsUpper(msg[i]))
                    {
                        msg = string.Concat(msg.AsSpan(pos, i), Strings.EmptySpace, msg.AsSpan(i));
                        pos = i + 1;
                        i++;
                    }
                }
            }          
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long,20).Show();
            });
            return msg;
        }
    }
}
