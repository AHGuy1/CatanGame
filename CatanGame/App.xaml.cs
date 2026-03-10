using CatanGame.Views;
namespace CatanGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new Check1();
            if (Current != null)
                Current.UserAppTheme = AppTheme.Light;
        }
    }
}
