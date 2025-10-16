using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterPageVM();
    }
}