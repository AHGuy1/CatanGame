using CommunityToolkit.Maui.Views;
using CatanGame.ModelsLogic;
using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class YearOfPlentyPage : Popup
{
	public YearOfPlentyPage(SpecialCards specialCards)
	{
		BindingContext = new YearOfPlentyPageVM(specialCards);
		InitializeComponent();
	}
}