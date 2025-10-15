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

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SaveToPreferences();
            }
            else
            {
                EmailIsTaken = true;
            }
        }

        private void LoginOnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {

            }
            else
            {

            }
        }

        private void SaveToPreferences()
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
