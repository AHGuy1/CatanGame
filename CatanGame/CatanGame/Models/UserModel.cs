using CatanGame.ModelsLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CatanGame.Models
{
    public abstract class UserModel
    {
        protected FbData fbd = new();
        public bool IsRegistered => !(string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(ConfirmPassword) && string.IsNullOrWhiteSpace(Email));
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;     
        public abstract void Register();
        public abstract bool Login();
    }
}
