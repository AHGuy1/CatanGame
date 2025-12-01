
using CatanGame.Models;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    public partial class BoradTestVM : ObservableObject
    {
        public BoradTestVM(Grid grdBorad)
        {
            Game game = new();
            game.Init(grdBorad);
            OnPropertyChanged(nameof(grdBorad));
        }
    }
}
