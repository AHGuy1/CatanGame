using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class HomePage : ContentPage
{
    private object? SOToRestore { get; set; }
    private readonly HomePageVM mpVM = new();
    public HomePage()
    {
        InitializeComponent();
        BindingContext = mpVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        mpVM.AddSnapshotListener();
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
        mpVM.RemoveSnapshotListener();
        base.OnDisappearing();
#if ANDROID
        if (Platform.CurrentActivity != null)
            if (SOToRestore is Android.Content.PM.ScreenOrientation SO) Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
#endif
    }
}