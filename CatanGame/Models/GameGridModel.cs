using CatanGame.ModelsLogic;
using System;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected IndexedButton[][] BoardPiceButtons = new IndexedButton[24][];
        protected IndexedImage[][] BoardPiceImages = new IndexedImage[24][];
        public EventHandler<IndexedButton>? ButtonClicked;
        public Image LongestRoad { get; set; } = new();
        public Image LargestArmy { get; set; } = new();
        protected Game game = new();

        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void HideButtuns();
        protected abstract void CheckLongestRoad();
        protected abstract int CheckLongestRoad(int row, int column, bool[][] visited);

        public abstract void OnChange();
        public abstract void Init(Grid gameBoard, Grid grdPices, Grid otherPices);
        public abstract void ShowBuildOptions(string peiceType);
    }
}
