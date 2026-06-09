using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Core;

namespace CatanGame.Models
{
    public abstract class UserModel
    {
        #region Fields
        protected FbData fbd = new();
        #endregion

        #region Events
        public EventHandler? AuthComplete;
        public EventHandler? AuthFalier;
        #endregion

        #region Properties
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        #endregion

        #region PublicMethods
        // Starts account registration.
        public abstract void Register();
        // Starts user login.
        public abstract void Login();
        // Logs out the current user.
        public abstract void LogOut();
        // Starts password reset.
        public abstract void ResetPassword();
        // Saves or clears remembered credentials.
        public abstract void RememberMe();
        #endregion

        #region PrivateMethods
        // Handles registration completion.
        protected abstract void RegisterOnComplete(Task task);
        // Handles password reset completion.
        protected abstract void ResetPasswordOnComplete(Task task);
        // Handles login completion.
        protected abstract void LoginOnComplete(Task task);
        #endregion
    }
}
