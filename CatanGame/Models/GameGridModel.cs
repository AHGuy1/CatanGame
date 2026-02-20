using CatanGame.ModelsLogic;
using IntelliJ.Lang.Annotations;
using SkiaSharp.Extended.UI.Controls;
using System;
using System.Windows.Input;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected IndexedButton[][] BoardPieceButtons = new IndexedButton[24][];
        protected IndexedImage[][] BoardPieceImages = new IndexedImage[24][];
        protected ImageButton[][] RoberImages = new ImageButton[5][];
        protected Game game = new();

        public EventHandler? EndTurnOnClicked;
        public SKLottieView Dice1Roll { get; set; } = new();
        public SKLottieView Dice2Roll { get; set; } = new();
        public Image Dice1Image { get; set; } = new();
        public Image Dice2Image { get; set; } = new();
        public Image LongestRoad { get; set; } = new();
        public Image LargestArmy { get; set; } = new();
        public Image BuildingCost { get; set; } = new();
        public Button RollButton { get; set; } = new();
        public Label RollLabel { get; set; } = new();

        protected abstract void OnBuildButtonClicked(object? sender, EventArgs e);
        protected abstract void OnRollButtonClicked(object? sender, EventArgs e);
        protected abstract void OnDiceUpdated(Task task);
        protected abstract void StartAnimations();
        protected abstract void StopAnimations();
        protected abstract void HideButtuns();
        protected abstract void CheckLongestRoad();
        protected abstract void ShowBuildOptions();
        protected abstract void EndTurn();
        protected abstract void OnRoberPlacementClicked(object? sender, EventArgs e);
        protected abstract int CheckLongestRoad(EdgeLink edge, bool[] visited);
        protected abstract int GetPieceIndexFromColor(int row, int column);
        protected abstract Grid CreateRoberImage(int row, int column);
        protected abstract BoardModel.PieceType GetPieceType(int row, int column);
        protected abstract bool CanShowBuildOptions();
        protected abstract bool CanEndTurn();

        public abstract void OnAnimationStatusChanged();
        public abstract void OnChange();
        public abstract void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame);
        public abstract void ShowBuildOptions(string pieceType);
    }
}
