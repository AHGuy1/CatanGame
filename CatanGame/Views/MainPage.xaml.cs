using CommunityToolkit.Maui.Views;
using CatanGame.ModelsLogic;
namespace CatanGame.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            Game game = new();
            TradePage popup = new(game);
            this.ShowPopup(popup);
        }
    }
}
