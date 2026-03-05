using CatanGame.Views;
namespace CatanGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new CatanGame.Views.MainPage();
            if (Current != null)
                Current.UserAppTheme = AppTheme.Light;
        }
    }
}
