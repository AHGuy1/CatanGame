using CatanGame.Models;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    public partial class BoardTestVM : ObservableObject
    {
        private readonly Game game = new();
        private readonly GameGrid ggrid = new(new Game());
        public string TimeLeft => game.TimeLeft;

        public BoardTestVM(Grid grdBoard, Grid grdPices)
        {
            game = new();
            ggrid.Init(grdBoard,grdPices);
            OnPropertyChanged(nameof(grdBoard));
            OnPropertyChanged(nameof(grdPices));
            game.TimeLeftChanged += UpdateTimeLeft;
        }

        private void UpdateTimeLeft(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimeLeft));
        }
    }
}
