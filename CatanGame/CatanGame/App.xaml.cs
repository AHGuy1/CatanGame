using CatanGame.ModelsLogic;
using CatanGame.Views;

namespace CatanGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new LogInPage();
        }
    }
}
