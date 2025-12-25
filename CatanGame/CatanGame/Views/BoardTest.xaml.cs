using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class BoardTest : ContentPage
{
    private object? SOToRestore { get; set; }
    public BoardTest()
	{
		InitializeComponent();
		BindingContext = new ViewModels.BoardTestVM(grdBoard, grdPices);
	}
    protected override void OnAppearing()
    {
#if ANDROID
        if (Platform.CurrentActivity != null)
        {
            SOToRestore = Platform.CurrentActivity.RequestedOrientation;
            Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
        }
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
#if ANDROID
        if (Platform.CurrentActivity != null)
            if (SOToRestore is Android.Content.PM.ScreenOrientation SO) Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
#endif
    }
}