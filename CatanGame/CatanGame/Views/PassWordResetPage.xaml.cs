using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class PassWordResetPage : ContentPage
{
	public PassWordResetPage()
	{
		InitializeComponent();
		BindingContext = new PassWordResetPageVM();
    }
}