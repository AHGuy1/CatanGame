using CatanGame.ViewModels;
using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Views;

namespace CatanGame.Views;

public partial class TradePage : Popup
{
    public TradePage(Game game)
    {
        InitializeComponent();
        BindingContext = new TradePageVM(game);
    }
}