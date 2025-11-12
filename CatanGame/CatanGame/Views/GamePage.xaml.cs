using CatanGame.ViewModels;
using CatanGame.ModelsLogic;
namespace CatanGame.Views;

public partial class GamePage : ContentPage
{
	public GamePage(Game game)
	{
		InitializeComponent();
		BindingContext = new GamePageVM(game);
	}
}