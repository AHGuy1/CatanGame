using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		BindingContext = new HomePageVM();

    }
}