using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CatanGame.ViewModels
{
    public partial class GamePageVM : ObservableObject
    {
        #region Fields
        private readonly Game game;
        private readonly GameGrid board;
        private readonly Animations animations;
        #endregion

        #region Properties
        public int PlayerCount => game.PlayerCount;
        public int PlayerIndector => game.PlayerIndicator;
        public double TimeOpacity => animations.TimeOpacity;
        public bool AvatarVisible => game.StatusMessage != GameStatus.Status.PleseWait.ToString();
        public bool IsBusy { get; set; } = false;
        public string StatusMessage => game.StatusMessage == Strings.YourTurn ? game.StatusMessage : PlayerNames[game.PlayerTurn - 1] + game.StatusMessage;
        public string AvatarUrl => game.PlayerAvatar.GetUrlWithString(PlayerNames[game.PlayerTurn - 1]);
        public string TimeLeft => game.TimeLeft;
        public string[] PlayerNames => game.PlayerNames;
        public Color StatusColor => game.StatusColor;
        public Color? TimeColor => animations.TimeColor;
        #endregion

        #region Constructor
        public GamePageVM(Game game, Grid grdBoard, Grid grdPieces, Grid otherPieces, Image frame, GamePage gamePage)
        {
            animations = new Animations();
            this.game = game;
            board = new(game);
            board.Init(grdBoard, grdPieces, otherPieces, frame, gamePage);
            board.EndTurnOnClicked += OnEndTurn;
            this.game.EndTurnOutOfTime += OutOfTimeEndTurn;
            this.game.GameDeleted += OnGameDeleted;
            this.game.PlayerLeft += OnPlayerLeft;
            this.game.GameChanged += OnGameChanged;
            this.game.TurnChanged += OnTurnChanged;
            this.game.TimeLeftChanged += UpdateTimeLeft;
            this.game.GridChanged += OnGridChanged;
            this.game.AnimationStatusChanged += OnAnimationStatusChanged;
            this.game.ResourceCountersUpdated += OnResourceCountersUpdated;
            this.game.TradeRecived += OnTradeRecived;
            this.game.CloseTradePopUp += OnCloseTradePopUp;
            animations.OpacityChanged += OnOpacityChanged;
            OnPropertyChanged(nameof(game.TimeLeft));
            OnPropertyChanged(nameof(board));
            game.StartGame();
        }
        #endregion

        #region Public Methods
        // Removes the active game listener.
        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
        #endregion

        #region Private Methods
        // Closes the trade popup from game events.
        private void OnCloseTradePopUp(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() => board.CloseTradePopUp());
        }

        // Opens the trade popup for a received trade.
        private void OnTradeRecived(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() => { board.TradeButton.Command.Execute(null); });
        }

        // Refreshes resource counters after resource changes.
        private void OnResourceCountersUpdated(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() => board.UpdateResourceCounters());
        }

        // Applies dice animation state changes.
        private void OnAnimationStatusChanged(object? sender, EventArgs e)
        {
            board.OnAnimationStatusChanged();
        }

        // Applies board state changes to the grid.
        private void OnGridChanged(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() => board.OnChange());
        }

        // Refreshes timer animation binding values.
        private void OnOpacityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimeColor));
            OnPropertyChanged(nameof(TimeOpacity));
        }

        // Updates the board when a new turn starts.
        private void OnTurnChanged(object? sender, EventArgs e)
        {
            board.OnTurnChanged();
        }

        // Refreshes the displayed time left.
        private void UpdateTimeLeft(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimeLeft));
        }

        // Shows timeout feedback and ends the turn.
        private void OutOfTimeEndTurn(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.OutOfTime, ToastDuration.Long, 20).Show();
            });
            EndTurn();
        }

        // Handles the board end turn event.
        private void OnEndTurn(object? sender, EventArgs e)
        {
            EndTurn();
        }

        // Returns home when the game is deleted.
        private void OnGameDeleted(object? sender, string messgae)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted + messgae, ToastDuration.Long, 20).Show();
                Application.Current!.MainPage = new AppShell();
            });
        }

        // Shows a message when another player leaves.
        private void OnPlayerLeft(object? sender, string message)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(message, ToastDuration.Long, 20).Show();
            });
        }

        // Refreshes game page bindings after game changes.
        private void OnGameChanged(object? sender, EventArgs e)
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(TimeLeft));
            OnPropertyChanged(nameof(AvatarUrl));
            OnPropertyChanged(nameof(AvatarVisible));
            OnPropertyChanged(nameof(StatusColor));
        }

        // Ensures the turn is complete and advances it.
        private void EndTurn()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));
            board.EnsurePlayerPlayed();
        }
        #endregion
    }
}
