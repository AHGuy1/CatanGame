namespace CatanGame.Views;

public partial class BoardTest : ContentPage
{
	public BoardTest()
	{
		InitializeComponent();
		BindingContext = new ViewModels.BoardTestVM(grdBoard, grdPices);
	}
}