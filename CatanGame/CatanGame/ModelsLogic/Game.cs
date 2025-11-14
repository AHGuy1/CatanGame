using CatanGame.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        protected override GameStatus Status => IsFull ? PlayerTurn == PlayerIndicator + 1 ? new GameStatus { CurrentStatus = GameStatus.Status.YourTurn } :
            PlayerTurn == 1 ? new GameStatus { CurrentStatus = GameStatus.Status.Player1Turn } :
            PlayerTurn == 2 ? new GameStatus { CurrentStatus = GameStatus.Status.Player2Turn } :
            PlayerTurn == 3 ? new GameStatus { CurrentStatus = GameStatus.Status.Player3Turn } :
            PlayerTurn == 4 ? new GameStatus { CurrentStatus = GameStatus.Status.Player4Turn } :
            PlayerTurn == 5 ? new GameStatus { CurrentStatus = GameStatus.Status.Player5Turn } :
            new GameStatus { CurrentStatus = GameStatus.Status.Player6Turn }
            : new GameStatus { CurrentStatus = GameStatus.Status.PleseWait };
        public Game(GameSize selectedGameSize)
        {
            PlayerCount = selectedGameSize.Size;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
        }
        public Game()
        {
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void GetDocument(string Id, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GamesCollection, Id , OnComplete);
        }

        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            for(int i = 0; i < PlayerCount; i++)
            {
                if (PlayerNames[i] == fbd.DisplayName)
                {
                    PlayerNames[i] = string.Empty;
                    if(i == 0)
                        DeleteDocument(OnComplete);
                    else
                    {
                        for( int k =0; k<PlayerCount-1; k++)
                        {
                            if (String.IsNullOrWhiteSpace(PlayerNames[k]))
                            {
                                PlayerNames[k] = PlayerNames[k + 1];
                                PlayerNames[k + 1] = string.Empty;
                            }
                        }
                        IsFull = false;
                        PlayerLeft = i;
                        SetDocument(OnCompletePlayerLeft);
                    }
                    i = PlayerCount;
                }
            }
        }

        private void OnCompletePlayerLeft(Task task)
        {
            OnPlayerLeft?.Invoke(this,PlayerLeft);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                IsFull = updatedGame.IsFull;
                PlayerNames = updatedGame.PlayerNames;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id);
            fbd.DeleteDocument(Keys.GameCodesCollection,GameCode,OnComplete);
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
