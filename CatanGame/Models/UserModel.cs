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
        public EventHandler? PhoneNumberFalier;
        public EventHandler? PhoneNumberComplete;
        public EventHandler? VerificationCodeFalier;
        public EventHandler? VerificationCodeComplete;
        #endregion

        #region Properties
        public bool IsRegistered { get; set; } = false;
        public string AvatarUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = Strings.PhoneAreaCode + Strings.EmptySpace;
        public string VerificationCode { get; set; } = string.Empty;
        #endregion

        #region PublicMethods
        public abstract void Register();
        public abstract void Login();
        public abstract void VerifyPhoneNumber();
        public abstract void LinkPhoneNumberToAcount();
        public abstract void SignInWithPhoneNumber();
        public abstract void ResetPassword();
        public abstract void RememberMe();
        #endregion

        #region PrivateMethods
        protected abstract void RegisterOnComplete(Task task);
        protected abstract void ResetPasswordOnComplete(Task task);
        protected abstract void LoginOnComplete(Task task);
        protected abstract void VerifyPhoneNumberOnComplete(Task task);
        protected abstract void SignInWithPhoneNumberOnComplete(Task task);
        protected abstract void LinkPhoneToAcountOnComplete(Task task);
        #endregion
    }
}
