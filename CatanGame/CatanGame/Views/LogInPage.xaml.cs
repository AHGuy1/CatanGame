using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class LogInPage : ContentPage
{
	public LogInPage()
	{
		InitializeComponent();
		BindingContext = new LogInPageVM();
	}
}