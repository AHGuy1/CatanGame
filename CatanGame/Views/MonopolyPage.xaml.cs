using CommunityToolkit.Maui.Views;
using CatanGame.ModelsLogic;
using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class MonopolyPage : Popup
{
	public MonopolyPage(SpecialCards specialCards)
	{
		BindingContext = new MonopolyPageVM(specialCards);
        InitializeComponent();
	}
}