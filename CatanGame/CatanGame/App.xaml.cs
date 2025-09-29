using CatanGame.Views;
using CatanGame.ModelsLogic;

namespace CatanGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            User user = new(true);
            Page page = user.IsRegistered ? new LogInPage() : new RegisterPage(); 
            MainPage  = page;
        }
    }
}
