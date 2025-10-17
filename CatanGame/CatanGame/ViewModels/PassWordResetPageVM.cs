using CatanGame.Models;
using System.Windows.Input;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    public partial class PassWordResetPageVM : ObservableObject
    {
        private readonly User user = new();
        public ICommand ResetPassWordCommand { get; }
        public bool IsVisibleEmailMessege { get; set; } = true;
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (ResetPassWordCommand as Command)?.ChangeCanExecute();
                ToggleIsVisibleEmailMessege();
            }
        }

        public PassWordResetPageVM()
        {
            ResetPassWordCommand = new Command(ResetPassWord, CanResetPassWord);
        }

        private void ToggleIsVisibleEmailMessege()
        {
            IsVisibleEmailMessege = !(user.Email.Contains('@') && user.Email.Contains('.'));
            OnPropertyChanged(nameof(IsVisibleEmailMessege));
        }
        private bool CanResetPassWord()
        {
            return user.Email.Contains('@') && user.Email.Contains('.');
        }
        private void ResetPassWord()
        {
            user.ResetPassword();
        }
    }
    
}
