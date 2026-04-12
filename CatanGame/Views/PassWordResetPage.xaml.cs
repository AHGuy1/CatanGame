using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class PasswordResetPage : ContentPage
{
    private object? SOToRestore { get; set; }
    public PasswordResetPage()
	{
		InitializeComponent();
		BindingContext = new PasswordResetPageVM();
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