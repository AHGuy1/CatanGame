using CatanGame.Models;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
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
                        for( int k =0; k<PlayerCount-1; i++)
                        {
                            if (String.IsNullOrWhiteSpace(PlayerNames[k]))
                            {
                                PlayerNames[k] = PlayerNames[k + 1];
                                PlayerNames[k + 1] = string.Empty;
                            }
                        }
                        IsFull = false;
                        SetDocument(OnComplete);
                    }
                    i = PlayerCount;
                }
            }
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

        private void OnComplete()
        {

        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
