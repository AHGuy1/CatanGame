using CatanGame.ViewModels;
using CatanGame.ModelsLogic;

namespace CatanGame.Views;

public partial class WaitingRoomPage : ContentPage
{
    private object? SOToRestore { get; set; }
    private readonly WaitingRoomPageVM gpVM;
    public WaitingRoomPage(Game game)
    {
        InitializeComponent();
        gpVM = new WaitingRoomPageVM(game);
        BindingContext = gpVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        gpVM.AddSnapshotListener();
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
        gpVM.RemoveSnapshotListener();
        base.OnDisappearing();
#if ANDROID
        if (Platform.CurrentActivity != null)
            if (SOToRestore is Android.Content.PM.ScreenOrientation SO) Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
#endif
    }
}