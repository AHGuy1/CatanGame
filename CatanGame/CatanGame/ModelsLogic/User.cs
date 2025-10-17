using CatanGame.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CatanGame.ModelsLogic
{
    public class User : UserModel
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, RegisterOnComplete);
        }

        public override void ResetPassword()
        {
            fbd.ResetPassword(Email, ResetPasswordOnComplete);
        }

        private void RegisterOnComplete(Task task)
        {
                if (task.IsCompletedSuccessfully)
                {
                    SaveToPreferences();
                    OnAuthComplete?.Invoke(this, EventArgs.Empty);
                }
                else if (task.Exception != null)
                {
                    string msg = task.Exception.Message;
                    ShowAlert(msg);
                }
                else
                    ShowAlert(Strings.UnknownError);           
        }
        private void ResetPasswordOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                
            }
            else if(task.Exception != null) 
            {
                string msg = task.Exception.Message;
                ShowAlert(msg);
            }
            else
                ShowAlert(Strings.UnknownError);
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
                ShowAlert(Strings.UnknownError);
        }

        private static void SaveToPreferences()
        {
            Preferences.Set(Keys.IsRegisteredKey, true);
        }
        private static void ShowAlert(string msg)
        {
            if (msg.Contains("INVALID_LOGIN_CREDENTIALS"))
                msg = Strings.InvalidCredentialsMessage;
            else if (msg.Contains("Reason"))
            {
                int pos = msg.IndexOf("Reason");
                msg = msg.Substring((pos + 7), msg.Length - pos - 8);
                for (int i = 1; i < msg.Length; i++)
                {
                    pos = 0;
                    if (char.IsUpper(msg[i]))
                    {
                        msg = string.Concat(msg.AsSpan(pos, i), " ", msg.AsSpan(i));
                        pos = i + 1;
                        i++;
                    }
                }
            }          
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long,20).Show();
            });        
        }
        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, LoginOnComplete);
            
        }
        public User()
        {
            IsRegistered = Preferences.Get(Keys.IsRegisteredKey, false);
        }
    }
}
