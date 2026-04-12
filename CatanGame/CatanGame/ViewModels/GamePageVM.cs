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
        public string[] PlayerNames => game.PlayerNames;
        public string StatusMessage => game.StatusMessage == Strings.YourTurn ? game.StatusMessage : PlayerNames[game.PlayerTurn - 1] + game.StatusMessage;
        public Color StatusColor => game.StatusColor;
        public string AvatarUrl => game.PlayerAvatar.GetUrlWithString(PlayerNames[game.PlayerTurn - 1]);
        public string TimeLeft => game.TimeLeft;
        public bool AvatarVisible => game.StatusMessage != GameStatus.Status.PleseWait.ToString();
        public bool IsBusy { get; set; } = false;
        public Color? TimeColor => animations.TimeColor;
        public double TimeOpacity => animations.TimeOpacity;
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
        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
        #endregion

        #region Private Methods
        private void OnCloseTradePopUp(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() => board.CloseTradePopUp());
        }

        private void OnTradeRecived(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() => { board.TradeButton.Command.Execute(null); });
        }

        private void OnResourceCountersUpdated(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() => board.UpdateResourceCounters());
        }

        private void OnAnimationStatusChanged(object? sender, EventArgs e)
        {
            board.OnAnimationStatusChanged();
        }

        private void OnGridChanged(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() => board.OnChange());
        }

        private void OnOpacityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimeColor));
            OnPropertyChanged(nameof(TimeOpacity));
        }

        private void OnTurnChanged(object? sender, EventArgs e)
        {
            board.OnTurnChanged();
        }

        private void UpdateTimeLeft(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimeLeft));
        }

        private void OutOfTimeEndTurn(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.OutOfTime, ToastDuration.Long, 20).Show();
            });
            EndTurn();
        }

        private void OnEndTurn(object? sender, EventArgs e)
        {
            EndTurn();
        }

        private void OnGameDeleted(object? sender, string messgae)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted + messgae, ToastDuration.Long, 20).Show();
                Application.Current!.MainPage = new AppShell();
            });
        }

        private void OnPlayerLeft(object? sender, string message)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(message, ToastDuration.Long, 20).Show();
            });
        }

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

        private void EndTurn()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));
            board.EnsurePlayerPlayed();
        }
        #endregion
    }
}
