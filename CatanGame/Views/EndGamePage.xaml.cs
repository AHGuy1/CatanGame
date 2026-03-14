using CatanGame.ModelsLogic;
using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class EndGamePage : ContentPage
{
	public EndGamePage(Game game)
	{
		BindingContext = new EndGamePageVM(game);
		InitializeComponent();
	}
}