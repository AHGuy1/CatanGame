using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public abstract class UserModel
    {
        protected FbData fbd = new();
        public bool EmailIsTaken { get; set; } = false;
        public bool IsRegistered { get; set; } = false;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public abstract void Register();
        public abstract bool Login();
    }
}
