using CatanGame.Models;
using System;

namespace CatanGame.ModelsLogic
{
    public class User : UserModel
    {
        public override void Register()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);

        }
        public override bool Login()
        {
            return true;
        }
        public User()
        {
            UserName = Preferences.Get(Keys.UserNameKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
            ConfirmPassword = Preferences.Get(Keys.PasswordKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);

        }
    }
}
