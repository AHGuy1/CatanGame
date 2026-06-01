using CatanGame.ModelsLogic;
using CatanGame.Views;
using IntelliJ.Lang.Annotations;
using Microsoft.Maui.Controls;
using SkiaSharp.Extended.UI.Controls;
using System;
using System.Windows.Input;
using static Android.InputMethodServices.Keyboard;

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

        #region Events
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

        #region Static Methods
        // Gets the fixed board tile and number for a grid position.
        protected static void GetFixedTile(int i, int k, out string sourceTile, out string sourceNumber) { sourceTile = i.ToString(); sourceNumber = k.ToString(); }

        // Gets the piece color name for a player number.
        protected static string GetPiecesColor(int i) => i.ToString();

        // Gets a screen-based size value for board controls.
        protected static double GetSizeProportion() => 0;

        // Creates a centered empty grid.
        protected static Grid CreateEmptyCenteredGrid() => [];

        // Creates a grid row used for card displays.
        protected static Grid CreateEmptyCardRowGrid() => [];

        // Creates a hidden looping dice roll animation.
        protected static SKLottieView CreateDiceAnimation() => new();

        // Creates an image for a board tile.
        protected static Image CreateTileImage(string imageSource) { return new() { Source = imageSource }; }

        // Creates a visible dice face image.
        protected static Image CreateDiceImage() => new();

        // Creates an image for a number token.
        protected static Image CreateNumberImage(string imageSource) { return new() { Source = imageSource }; }

        // Creates a road piece image with the given rotation.
        protected static Image CreateRoadImage(int rotation) { return new() { Rotation = rotation }; }

        // Creates an image for a settlement or city position.
        protected static Image CreateApexImage() => new();

        // Creates a label for resource and card counters.
        protected static Label CreateCardLabel() => new();

        // Creates a clickable road position.
        protected static IndexedButton CreateRoadButton(int rotation, int colmnIndex, int rowIndex) => new(rowIndex, colmnIndex, 0, 0, rotation);

        // Creates a clickable settlement or city position.
        protected static IndexedButton CreateApexButton(int colmnIndex, int rowIndex) => new(rowIndex, colmnIndex, 0, 0);

        // Gets the dice image source for a roll value.
        protected static string GetDiceImage(int dice) { return dice.ToString(); }

        // Creates a display image for a resource or bonus card.
        public static Image CreateCardImage(string source) { return new() { Source = source }; }

        // Converts a tile row and column to the hex array index.
        public static int GetTileLocationInArray(int row, int column) { return row + column; }

        // Gets the number of hex tiles in a board row.
        public static int GetAmountOfColumnsTiles(int i) => i;

        // Gets the number of piece columns in a board row.
        public static int GetAmountOfColumns(int i) => i;

        // Converts a piece row and column to its array index.
        public static int GetPieceLocationInArray(int row, int column) { return row + column; }
        #endregion

        #region PublicMethods
        // Closes the active trade popup.
        public abstract void CloseTradePopUp();
        // Refreshes displayed resource counters.
        public abstract void UpdateResourceCounters();
        // Completes required turn actions if time expires.
        public abstract void EnsurePlayerPlayed();
        // Applies dice animation state changes.
        public abstract void OnAnimationStatusChanged();
        // Applies changed game state to the board.
        public abstract void OnChange();
        // Prepares the UI for a new turn.
        public abstract void OnTurnChanged();
        // Makes one robber placement tile selectable.
        public abstract void SetVisibleRobberImages(int row, int column);
        // Builds the game board UI.
        public abstract void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame, GamePage gamePage);
        // Shows valid build positions for a piece type.
        public abstract void ShowBuildOptions(string pieceType);
        #endregion

        #region PrivateMethods
        // Handles a selected build position.
        protected abstract void OnBuildButtonClicked(object? sender, EventArgs e);
        // Handles a dice roll button click.
        protected abstract void OnRollButtonClicked(object? sender, EventArgs e);
        // Handles a selected robber placement tile.
        protected abstract void OnRobberPlacementClicked(object? sender, EventArgs e);
        // Uses a development card.
        protected abstract void UseCard(object paramter);
        // Places a town at a board position.
        protected abstract void BuildTown(int row, int column);
        // Places a road at a board position.
        protected abstract void BuildRoad(int row, int column);
        // Checks and grants harbor ownership.
        protected abstract void CheckIfOnHarbor(int row, int column);
        // Handles dice update completion.
        protected abstract void OnDiceUpdated(Task task);
        // Opens the trade flow.
        protected abstract void Trade();
        // Buys a development card.
        protected abstract void GetCardFromPackege();
        // Rolls both dice.
        protected abstract void RollDice();
        // Starts dice animations.
        protected abstract void StartAnimations();
        // Stops dice animations.
        protected abstract void StopAnimations();
        // Hides build buttons.
        protected abstract void HideButtuns();
        // Hides robber placement buttons.
        protected abstract void HideRobberButtuns();
        // Recalculates longest road.
        protected abstract void CheckLongestRoad();
        // Shows all available build options.
        protected abstract void ShowBuildOptions();
        // Builds a town at the first valid option.
        protected abstract void BuildTownAtFirstPosition();
        // Builds a road at the first valid option.
        protected abstract void BuildRoadAtFirstPosition();
        // Shows valid robber placement options.
        protected abstract void ShowRobberPlacmentOptions();
        // Raises or performs end turn behavior.
        protected abstract void EndTurn();
        // Syncs board pieces.
        protected abstract void UpdateBoardPices();
        // Refreshes development card counters.
        protected abstract void UpdateSpecialCards();
        // Measures longest road from one edge.
        protected abstract int CheckLongestRoad(EdgeLink edge, bool[] visited);
        // Gets owner index from a piece color.
        protected abstract int GetPieceIndexFromColor(int row, int column);
        // Checks whether a development card can be used.
        protected abstract bool CenUseCard(object paramter);
        // Checks whether build options can be shown.
        protected abstract bool CanShowBuildOptions();
        // Checks whether the turn can end.
        protected abstract bool CanEndTurn();
        // Checks whether trading can start.
        protected abstract bool CenTrade();
        // Checks whether a development card can be bought.
        protected abstract bool CenGetCardFromPackege();
        // Creates a robber image control.
        protected abstract Grid CreateRobberImage(int row, int column);
        // Creates a development card button.
        protected abstract ImageButton CreateCardImageButton(string source);
        // Gets the piece type at a board position.
        protected abstract BoardModel.PieceType GetPieceType(int row, int column);
        #endregion
    }
}
