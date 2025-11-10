using CatanGame.ModelsLogic;
using CatanGame.Models;
using CommunityToolkit.Maui.Alerts;

namespace CatanGame.ViewModels
{
    public class GamePageVM
    {
        private readonly Game game;
        public string[] PlayerNames => game.PlayerNames;
        public int PlayerCount => game.PlayerCount;
        public string MyName => game.MyName;
        public string OpponentName => game.OpponentName;
        public GamePageVM(Game game)
        {
            this.game = game;
            if (!game.IsHost)
            {
                game.GuestName = MyName;
                game.IsFull = true;
                game.SetDocument(OnComplete);
            }
        }

        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameErr, CommunityToolkit.Maui.Core.ToastDuration.Long, 14);

        }
    }
}
