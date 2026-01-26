using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class RegisterPage : ContentPage
{
    private object? SOToRestore { get; set; }
    public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterPageVM();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
#if ANDROID
        if (Platform.CurrentActivity != null)
        {
            SOToRestore = Platform.CurrentActivity.RequestedOrientation;
            Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
        }
#endif
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
#if ANDROID
        if (Platform.CurrentActivity != null)
            if (SOToRestore is Android.Content.PM.ScreenOrientation SO) Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
#endif
    }
}