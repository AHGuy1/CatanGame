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
        public abstract void Register();
        public abstract void Login();
        public abstract void ResetPassword();
        public abstract void RememberMe();
        #endregion

        #region PrivateMethods
        protected abstract void RegisterOnComplete(Task task);
        protected abstract void ResetPasswordOnComplete(Task task);
        protected abstract void LoginOnComplete(Task task);
        #endregion
    }
}
