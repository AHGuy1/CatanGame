using CatanGame.Models;
using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CatanGame.ViewModels
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        private readonly GameGrid board;
        private readonly Animations animations;
        public int PlayerCount => game.PlayerCount;
        public int PlayerIndector => game.PlayerIndicator;
        public string[] PlayerNames => game.PlayerNames;
        public string StatusMessage => game.StatusMessage == Strings.YourTurn ? game.StatusMessage : PlayerNames[game.PlayerTurn-1] + game.StatusMessage;
        public Color StatusColor => game.StatusColor;
        public string AvatarUrl => game.PlayerAvatar.GetUrlWithString(PlayerNames[game.PlayerTurn - 1]);
        public string TimeLeft => game.TimeLeft;
        public bool AvatarVisible => game.StatusMessage != GameStatus.Status.PleseWait.ToString() ;
        public bool ShouldGameBeDeleted = true;
        public bool IsBusy { get; set; } = false;
        public Color? TimeColor => animations.TimeColor;
        public double TimeOpacity => animations.TimeOpacity;

        public GamePageVM(Game game, Grid grdBoard, Grid grdPieces, Grid otherPieces, Image frame)
        {
            animations = new Animations();
            this.game = game;
            board = new(game);
            board.Init(grdBoard, grdPieces, otherPieces,frame);
            board.EndTurnOnClicked += EndTurn;
            this.game.EndTurnOutOfTime += OutOfTimeEndTurn;
            this.game.GameDeleted += OnGameDeleted;
            this.game.PlayerLeft += OnPlayerLeft;
            this.game.GameChanged += OnGameChanged;
            this.game.TurnChanged += OnTurnChanged;
            this.game.TimeLeftChanged += UpdateTimeLeft;
            this.game.GridChanged += OnGridChanged;
            this.game.AnimationStatusChanged += OnAnimationStatusChanged;
            animations.OpacityChanged += OnOpacityChanged;
            OnPropertyChanged(nameof(game.TimeLeft));
            OnPropertyChanged(nameof(board));
            game.StartGame();
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
            if (game.Turn <= game.PlayerCount*2)
            {
                if(game.PlayerTurn == game.PlayerIndicator + 1)
                    board.ShowBuildOptions(Strings.Town);
            }
            else if (game.PlayerTurn == game.PlayerIndicator + 1)
                board.RollButton.IsEnabled = true;

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
        private void EndTurn(object? sender, EventArgs e)
        {
            EndTurn();
        }
        private void OnGameDeleted(object? sender, string messgae)
        {
            ShouldGameBeDeleted = false;
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted + messgae, ToastDuration.Long, 20).Show();
                Application.Current!.MainPage = new AppShell();
            });
        }
        private void OnPlayerLeft(object? sender, int Player)
        {
            if (Player == 1)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player2Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 2)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player3Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 3)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player4Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 4)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player5Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 5)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player6Left, ToastDuration.Long, 20).Show();
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

        public void RemoveSnapshotListener()
        {
            if(ShouldGameBeDeleted)
                game.RemoveSnapshotListener();
        }
    }
}
