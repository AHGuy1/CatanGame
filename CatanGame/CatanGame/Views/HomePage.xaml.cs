using CatanGame.ViewModels;

namespace CatanGame.Views;

public partial class HomePage : ContentPage
{
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
    }

    protected override void OnDisappearing()
    {
        mpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }
}