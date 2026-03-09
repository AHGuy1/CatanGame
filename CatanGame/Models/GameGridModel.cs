using CatanGame.ModelsLogic;
using CatanGame.Views;
using IntelliJ.Lang.Annotations;
using SkiaSharp.Extended.UI.Controls;
using System;
using System.Windows.Input;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected GamePage? CurrentGamePage;
        protected TradePage? CurrentTradePopUp;
        protected IndexedButton[][] BoardPieceButtons = new IndexedButton[24][];
        protected Image[][] BoardPieceImages = new Image[24][];
        protected ImageButton[][] RobberImages = new ImageButton[5][];
        protected ICommand? ShowBuildOptionsCommand { get; set; }
        protected ICommand? EndTurnCommand { get; set; }

        public Game Game = new();
        public EventHandler? EndTurnOnClicked;
        public SKLottieView Dice1Roll { get; set; } = new();
        public SKLottieView Dice2Roll { get; set; } = new();
        public Image Dice1Image { get; set; } = new();
        public Image Dice2Image { get; set; } = new();
        public Image LongestRoad { get; set; } = new();
        public Image LargestArmy { get; set; } = new();
        public Button RollButton { get; set; } = new();
        public Button TradeButton { get; set; } = new();
        public Label RollLabel { get; set; } = new();
        public Label WoodCountLabel { get; set; } = new();
        public Label BrickCountLabel { get; set; } = new();
        public Label SheepCountLabel { get; set; } = new();
        public Label WheatCountLabel { get; set; } = new();
        public Label OreCountLabel { get; set; } = new();
        protected abstract void OnBuildButtonClicked(object? sender, EventArgs e);
        protected abstract void OnRollButtonClicked(object? sender, EventArgs e);
        protected abstract void OnRobberPlacementClicked(object? sender, EventArgs e);
        protected abstract void BuildTown(int row, int column);
        protected abstract void BuildRoad(int row, int column);
        protected abstract void CheckIfOnHarbor(int row, int column);
        protected abstract void OnDiceUpdated(Task task);
        protected abstract void Trade();
        protected abstract void RollDice();
        protected abstract void StartAnimations();
        protected abstract void StopAnimations();
        protected abstract void HideButtuns();
        protected abstract void HideRobberButtuns();
        protected abstract void CheckLongestRoad();
        protected abstract void ShowBuildOptions();
        protected abstract void BuildTownAtFirstPosition();
        protected abstract void BuildRoadAtFirstPosition();
        protected abstract void ShowRobberPlacmentOptions();
        protected abstract void EndTurn();
        protected abstract void UpdateBoardPices();
        protected abstract int CheckLongestRoad(EdgeLink edge, bool[] visited);
        protected abstract int GetPieceIndexFromColor(int row, int column);
        protected abstract bool CanShowBuildOptions();
        protected abstract bool CanEndTurn();
        protected abstract bool CenTrade();
        protected abstract Grid CreateRobberImage(int row, int column);
        protected abstract BoardModel.PieceType GetPieceType(int row, int column);
        public abstract void CloseTradePopUp();
        public abstract void UpdateResourceCounters();
        public abstract void EnsurePlayerPlayed();
        public abstract void OnAnimationStatusChanged();
        public abstract void OnChange();
        public abstract void SetVisibleRobberImages(int row, int column);
        public abstract void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame, GamePage gamePage);
        public abstract void ShowBuildOptions(string pieceType);
    }
}
