using CommunityToolkit.Maui.Views;

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
            TradePage popup = new();
            this.ShowPopup(popup);
        }
    }
}
