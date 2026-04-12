using CatanGame.ModelsLogic;
using CatanGame.Views;
using IntelliJ.Lang.Annotations;
using SkiaSharp.Extended.UI.Controls;
using System;
using System.Windows.Input;

namespace CatanGame.Models
{
    public abstract class GameGridModel
    {
        #region Fields
        protected Game Game = new();
        protected SpecialCards SpecialCards = new();
        protected Board BoardData = new();
        protected TradePage? CurrentTradePopUp;
        protected IndexedButton[][] BoardPieceButtons = new IndexedButton[24][];
        protected Image[][] BoardPieceImages = new Image[24][];
        protected ImageButton[][] RobberImages = new ImageButton[5][];
        //Index 0 = Wood, Index 1 = Brick, Index 2 = Sheep, Index 3 = Wheat, Index 4 = Ore
        //Index 5 = Knight, Index 6 = University, Index 7 = Road Building, Index 8 = Monopoley, Index 9 = YearOfPlenty
        protected Label[] Counters { get; set; } = new Label[10];
        //Index 0 = Knight, Index 1 = University, Index 2 = Road Building, Index 3 = Monopoley, Index 4 = YearOfPlenty, Index 5 = CardBackGroud
        protected ImageButton[] SpecialCardImages = new ImageButton[6];
        protected SKLottieView Dice1Roll { get; set; } = new();
        protected SKLottieView Dice2Roll { get; set; } = new();
        protected Image Dice1Image { get; set; } = new();
        protected Image Dice2Image { get; set; } = new();
        protected Image LongestRoad { get; set; } = new();
        protected Image LargestArmy { get; set; } = new();
        protected Label RollLabel { get; set; } = new();
        #endregion

        #region Eventscxd
        public EventHandler? EndTurnOnClicked;
        #endregion

        #region Commands
        protected ICommand? ShowBuildOptionsCommand { get; set; }
        protected ICommand? EndTurnCommand { get; set; }
        #endregion

        #region Properties
        public GamePage? CurrentGamePage;
        public Button TradeButton { get; set; } = new();
        public Button RollButton { get; set; } = new();
        #endregion

        #region PublicMethods
        public abstract void CloseTradePopUp();
        public abstract void UpdateResourceCounters();
        public abstract void EnsurePlayerPlayed();
        public abstract void OnAnimationStatusChanged();
        public abstract void OnChange();
        public abstract void OnTurnChanged();
        public abstract void SetVisibleRobberImages(int row, int column);
        public abstract void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame, GamePage gamePage);
        public abstract void ShowBuildOptions(string pieceType);
        #endregion

        #region PrivateMethods
        protected abstract void OnBuildButtonClicked(object? sender, EventArgs e);
        protected abstract void OnRollButtonClicked(object? sender, EventArgs e);
        protected abstract void OnRobberPlacementClicked(object? sender, EventArgs e);
        protected abstract void UseCard(object paramter);
        protected abstract void BuildTown(int row, int column);
        protected abstract void BuildRoad(int row, int column);
        protected abstract void CheckIfOnHarbor(int row, int column);
        protected abstract void OnDiceUpdated(Task task);
        protected abstract void Trade();
        protected abstract void GetCardFromPackege();
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
        protected abstract void UpdateSpecialCards();
        protected abstract int CheckLongestRoad(EdgeLink edge, bool[] visited);
        protected abstract int GetPieceIndexFromColor(int row, int column);
        protected abstract bool CenUseCard(object paramter);
        protected abstract bool CanShowBuildOptions();
        protected abstract bool CanEndTurn();
        protected abstract bool CenTrade();
        protected abstract bool CenGetCardFromPackege();
        protected abstract Grid CreateRobberImage(int row, int column);
        protected abstract ImageButton CreateCardImageButton(string source);
        protected abstract BoardModel.PieceType GetPieceType(int row, int column);
        #endregion
    }
}
