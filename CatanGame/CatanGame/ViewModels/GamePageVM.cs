using CatanGame.ModelsLogic;
using CatanGame.Models;
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
        public string Playr1Name =>  PlayerCount > 0 ? PlayerNames[0] : string.Empty;
        public string Playr2Name => PlayerCount > 1 ? PlayerNames[1] : string.Empty;
        public string Playr3Name => PlayerCount > 2 ? PlayerNames[2] : string.Empty;
        public string Playr4Name => PlayerCount > 3 ? PlayerNames[3] : string.Empty;
        public string Playr5Name => PlayerCount > 4 ? PlayerNames[4] : string.Empty;
        public string Playr6Name => PlayerCount > 5 ? PlayerNames[5] : string.Empty;
        public string GameCode => game.GameCode;
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
        }

        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);

        }
    }
}
