using CatanGame.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace CatanGame.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterPageVM();
    }
}