using CatanGame.ViewModels;
namespace CatanGame.Views;

public partial class Check1 : ContentPage
{
	public Check1()
	{
		InitializeComponent();
        BindingContext = new Check1VM();
    }
}