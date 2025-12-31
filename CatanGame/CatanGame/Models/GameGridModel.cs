using CatanGame.ModelsLogic;
using System;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected IndexedButton[][] BoardPiceButtons = new IndexedButton[23][];
        public EventHandler<IndexedButton>? ButtonClicked;

        protected abstract IndexedButton CreateRoadButton(int rotation, int colmnIndex, int rowIndex);
        protected abstract IndexedButton CreateApexButton(int colmnIndex, int rowIndex);
        protected abstract void OnButtonClicked(object? sender, EventArgs e);

        public abstract void Init(Grid gameBoard, Grid grdPices, Game game);
        public abstract void ShowBuildOptions(string peiceType, Game game);
    }
}
