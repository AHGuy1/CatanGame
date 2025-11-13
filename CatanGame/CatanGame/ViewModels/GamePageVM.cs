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
        public string Playr1Name =>  PlayerCount > 0 ? Strings.Playr1Host + PlayerNames[0] : string.Empty;
        public string Playr2Name => PlayerCount > 1 ? Strings.Playr2 + PlayerNames[1] : string.Empty;
        public string Playr3Name => PlayerCount > 2 ? Strings.Playr3 + PlayerNames[2] : string.Empty;
        public string Playr4Name => PlayerCount > 3 ? Strings.Playr4 + PlayerNames[3] : string.Empty;
        public string Playr5Name => PlayerCount > 4 ? Strings.Playr5 + PlayerNames[4] : string.Empty;
        public string Playr6Name => PlayerCount > 5 ? Strings.Playr6 + PlayerNames[5] : string.Empty;
        public bool IsVisiblePlayr3Visible => PlayerCount > 2;
        public bool IsVisiblePlayr4Visible => PlayerCount > 3;
        public bool IsVisiblePlayr5Visible => PlayerCount > 4;
        public bool IsVisiblePlayr6Visible => PlayerCount > 5;
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
            game.OnGameChanged += OnGameChanged;
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Playr1Name));
            OnPropertyChanged(nameof(Playr2Name));
            OnPropertyChanged(nameof(Playr3Name));
            OnPropertyChanged(nameof(Playr4Name));
            OnPropertyChanged(nameof(Playr5Name));
            OnPropertyChanged(nameof(Playr6Name));
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
