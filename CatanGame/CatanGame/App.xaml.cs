using CatanGame.ModelsLogic;
using CatanGame.Views;

namespace CatanGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            User user = new();
            Page page = user.IsRegistered ? new LogInPage() : new RegisterPage();
            MainPage = page;
        }
    }
}
