
using CatanGame.Models;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    public partial class BoardTestVM : ObservableObject
    {
        public BoardTestVM(Grid grdBoard, Grid grdPices)
        {
            Game game = new();
            game.Init(grdBoard,grdPices);
            OnPropertyChanged(nameof(grdBoard));
            OnPropertyChanged(nameof(grdPices));
        }
    }
}
