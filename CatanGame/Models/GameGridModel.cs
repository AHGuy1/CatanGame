using CatanGame.ModelsLogic;
using SkiaSharp.Extended.UI.Controls;
using System;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected IndexedButton[][] BoardPieceButtons = new IndexedButton[24][];
        protected IndexedImage[][] BoardPieceImages = new IndexedImage[24][];
        protected ImageButton[][] RoberImages = new ImageButton[5][];
        public SKLottieView Dice1Roll { get; set; } = new();
        public SKLottieView Dice2Roll { get; set; } = new();
        public Image Dice1Image { get; set; } = new();
        public Image Dice2Image { get; set; } = new();
        public Image LongestRoad { get; set; } = new();
        public Image LargestArmy { get; set; } = new();
        public Button RollButton { get; set; } = new();
        public Label RollLabel { get; set; } = new();
        protected Game game = new();

        protected abstract void OnBuildButtonClicked(object? sender, EventArgs e);
        protected abstract void OnRollButtonClicked(object? sender, EventArgs e);
        protected abstract void OnDiceUpdated(Task task);
        protected abstract void StartAnimations();
        protected abstract void StopAnimations();
        protected abstract void HideButtuns();
        protected abstract void CheckLongestRoad();
        protected abstract int CheckLongestRoad(EdgeLink edge, bool[] visited);
        protected abstract int GetPieceIndexFromColor(int row, int column);
        protected abstract BoardModel.PieceType GetPieceType(int row, int column);

        public abstract void OnAnimationStatusChanged();
        public abstract void OnChange();
        public abstract void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame);
        public abstract void ShowBuildOptions(string pieceType);
    }
}
