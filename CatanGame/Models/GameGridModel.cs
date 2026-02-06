using CatanGame.ModelsLogic;
using System;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected IndexedButton[][] BoardPieceButtons = new IndexedButton[24][];
        protected IndexedImage[][] BoardPieceImages = new IndexedImage[24][];
        public EventHandler<IndexedButton>? ButtonClicked;
        public Image LongestRoad { get; set; } = new();
        public Image LargestArmy { get; set; } = new();
        protected Game game = new();

        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void HideButtuns();
        protected abstract void CheckLongestRoad();
        protected abstract int CheckLongestRoad(EdgeLink edge, bool[] visited);
        protected abstract int GetPieceIndexFromColor(int row, int column);
        protected abstract BoardModel.PieceType GetPieceType(int row, int column);

        public abstract void OnChange();
        public abstract void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame);
        public abstract void ShowBuildOptions(string pieceType);
    }
}
