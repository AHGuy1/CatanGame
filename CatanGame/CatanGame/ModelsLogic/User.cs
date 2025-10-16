using CatanGame.Models;
using CatanGame.ViewModels;
using System;

namespace CatanGame.ModelsLogic
{
    public class User : UserModel
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }

        public override void ResetPassword()
        {
            fbd.ResetPassword(Email, ResetPasswordOnComplete);
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SaveToPreferences();
            }
            else
            {
                InvalidEmailOrPassword = true;
            }
        }
        private void ResetPasswordOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                
            }
            else
            {
                InvalidEmailOrPassword = true;
            }
        }

        private void LoginOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {

            }
            else
            {
                InvalidEmailOrPassword = true;
            }
        }

        private static void SaveToPreferences()
        {
            Preferences.Set(Keys.IsRegisteredKey, true);
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
