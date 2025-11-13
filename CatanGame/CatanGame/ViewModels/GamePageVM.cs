using Android.Content;
using CatanGame.Models;
using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CatanGame.ViewModels
{
    public class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public bool IsBusy => false;
        public int PlayerCount => game.PlayerCount;
        public string[] PlayerNames => game.PlayerNames;
        public string Player1Name =>  PlayerCount > 0 ? Strings.Player1Host + PlayerNames[0] : string.Empty;
        public string Player2Name => PlayerCount > 1 ? Strings.Player2 + PlayerNames[1] : string.Empty;
        public string Player3Name => PlayerCount > 2 ? Strings.Player3 + PlayerNames[2] : string.Empty;
        public string Player4Name => PlayerCount > 3 ? Strings.Player4 + PlayerNames[3] : string.Empty;
        public string Player5Name => PlayerCount > 4 ? Strings.Player5 + PlayerNames[4] : string.Empty;
        public string Player6Name => PlayerCount > 5 ? Strings.Player6 + PlayerNames[5] : string.Empty;
        public bool IsVisiblePlayer3Visible => PlayerCount > 2;
        public bool IsVisiblePlayer4Visible => PlayerCount > 3;
        public bool IsVisiblePlayer5Visible => PlayerCount > 4;
        public bool IsVisiblePlayer6Visible => PlayerCount > 5;
        public string GameCode => Strings.GameCode + game.GameCode;
        public GamePageVM(Game game)
        {
            this.game = game;
            for (int i = 0; i < PlayerCount; i++)
            {
                if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                {
                    FbData fbd = new FbData();
                    PlayerNames[i] = fbd.DisplayName;
                    if(i+1 == PlayerCount)
                        game.IsFull = true;
                    game.SetDocument(OnComplete);
                    i = PlayerCount; 
                }
            }
            game.OnGameDeleted =
            game.OnGameChanged += OnGameChanged;
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Player1Name));
            OnPropertyChanged(nameof(Player2Name));
            OnPropertyChanged(nameof(Player3Name));
            OnPropertyChanged(nameof(Player4Name));
            OnPropertyChanged(nameof(Player5Name));
            OnPropertyChanged(nameof(Player6Name));
        }

        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);

        }

        public void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
    }
}
