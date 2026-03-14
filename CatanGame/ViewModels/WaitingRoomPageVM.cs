using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class WaitingRoomPageVM : ObservableObject
    {
        #region Fields
        private readonly Game game;
        #endregion

        #region Commands
        public ICommand StartGameCommand { get; }
        #endregion

        #region Properties
        public int PlayerCount => game.PlayerCount;
        public int PlayerIndector => game.PlayerIndicator;
        public string[] PlayerNames => game.PlayerNames;
        public string StatusMessage => game.StatusMessage;
        public string GameCode => Strings.GameCode + game.GameCode;
        public string Player1Name => PlayerCount > 0 ? PlayerIndector == 0 ? Strings.Player1Host + PlayerNames[0] + Strings.You : Strings.Player1Host + PlayerNames[0] : string.Empty;
        public string Player2Name => PlayerCount > 1 ? PlayerIndector == 1 ? Strings.Player2 + PlayerNames[1] + Strings.You : Strings.Player2 + PlayerNames[1] : string.Empty;
        public string Player3Name => PlayerCount > 2 ? PlayerIndector == 2 ? Strings.Player3 + PlayerNames[2] + Strings.You : Strings.Player3 + PlayerNames[2] : string.Empty;
        public string Player4Name => PlayerCount > 3 ? PlayerIndector == 3 ? Strings.Player4 + PlayerNames[3] + Strings.You : Strings.Player4 + PlayerNames[3] : string.Empty;
        public string Player5Name => PlayerCount > 4 ? PlayerIndector == 4 ? Strings.Player5 + PlayerNames[4] + Strings.You : Strings.Player5 + PlayerNames[4] : string.Empty;
        public string Player6Name => PlayerCount > 5 ? PlayerIndector == 5 ? Strings.Player6 + PlayerNames[5] + Strings.You : Strings.Player6 + PlayerNames[5] : string.Empty;
        public string AvatarUrl1 => PlayerCount > 0 ? !String.IsNullOrWhiteSpace(PlayerNames[0]) ? game.PlayerAvatar.GetUrlWithString(PlayerNames[0]) : string.Empty : string.Empty;
        public string AvatarUrl2 => PlayerCount > 1 ? !String.IsNullOrWhiteSpace(PlayerNames[1]) ? game.PlayerAvatar.GetUrlWithString(PlayerNames[1]) : string.Empty : string.Empty;
        public string AvatarUrl3 => PlayerCount > 2 ? !String.IsNullOrWhiteSpace(PlayerNames[2]) ? game.PlayerAvatar.GetUrlWithString(PlayerNames[2]) : string.Empty : string.Empty;
        public string AvatarUrl4 => PlayerCount > 3 ? !String.IsNullOrWhiteSpace(PlayerNames[3]) ? game.PlayerAvatar.GetUrlWithString(PlayerNames[3]) : string.Empty : string.Empty;
        public string AvatarUrl5 => PlayerCount > 4 ? !String.IsNullOrWhiteSpace(PlayerNames[4]) ? game.PlayerAvatar.GetUrlWithString(PlayerNames[4]) : string.Empty : string.Empty;
        public string AvatarUrl6 => PlayerCount > 5 ? !String.IsNullOrWhiteSpace(PlayerNames[5]) ? game.PlayerAvatar.GetUrlWithString(PlayerNames[5]) : string.Empty : string.Empty;
        public bool IsBusy { get; set; } = false;
        public bool IsEnabled => !IsBusy;
        public bool IsVisiblePlayer3Visible => PlayerCount > 2;
        public bool IsVisiblePlayer4Visible => PlayerCount > 3;
        public bool IsVisiblePlayer5Visible => PlayerCount > 4;
        public bool IsVisiblePlayer6Visible => PlayerCount > 5;
        #endregion

        #region Constructor
        public WaitingRoomPageVM(Game game)
        {
            StartGameCommand = new Command(StartGame, CanStartGame);
            this.game = game;
            this.game.AddPlayerName();
            this.game.GameDeleted += OnGameDeleted;
            this.game.PlayerLeft += OnPlayerLeft;
            this.game.GameChanged += OnGameChanged;
        }
        #endregion

        #region Public Methods
        public void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            if (!game.GameStarted)
                game.RemoveSnapshotListener();
        }
        #endregion

        #region Private Methods
        private void OnGameDeleted(object? sender, string message)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted + message, ToastDuration.Long, 20).Show();
                Application.Current!.MainPage = new AppShell();
            });
        }

        private void OnPlayerLeft(object? sender, int Player)
        {
            if (PlayerIndector != Player)
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
        }

        private bool CanStartGame()
        {
            return true;
        }

        private void StartGame()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current!.MainPage = new GamePage(game);
            });
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(Player1Name));
            OnPropertyChanged(nameof(Player2Name));
            OnPropertyChanged(nameof(Player3Name));
            OnPropertyChanged(nameof(Player4Name));
            OnPropertyChanged(nameof(Player5Name));
            OnPropertyChanged(nameof(Player6Name));
            OnPropertyChanged(nameof(AvatarUrl1));
            OnPropertyChanged(nameof(AvatarUrl2));
            OnPropertyChanged(nameof(AvatarUrl3));
            OnPropertyChanged(nameof(AvatarUrl4));
            OnPropertyChanged(nameof(AvatarUrl5));
            OnPropertyChanged(nameof(AvatarUrl6));
            OnPropertyChanged(nameof(StatusMessage));
            (StartGameCommand as Command)?.ChangeCanExecute();
        }
        #endregion
    }
}
