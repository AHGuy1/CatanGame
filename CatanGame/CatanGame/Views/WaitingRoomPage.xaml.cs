using CatanGame.ViewModels;
using CatanGame.ModelsLogic;

namespace CatanGame.Views;

public partial class WaitingRoomPage : ContentPage
{
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
    }

    protected override void OnDisappearing()
    {
        gpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }
}