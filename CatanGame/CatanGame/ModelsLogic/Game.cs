using CatanGame.Models;
using CatanGame.Views;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        protected override GameStatus Status => _status;
        public Game(GameSize selectedGameSize)
        {
            PlayerCount = selectedGameSize.Size;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            UpdateStatus();
        }
        public Game()
        {
            UpdateStatus();
        }
        protected override void UpdateStatus()
        {
            _status.CurrentStatus = !GameStarted ? GameStatus.Status.PleseWait :
                PlayerTurn == PlayerIndicator + 1 ? GameStatus.Status.YourTurn :
                PlayerTurn == 1 ? GameStatus.Status.Player1Turn :
                PlayerTurn == 2 ? GameStatus.Status.Player2Turn :
                PlayerTurn == 3 ? GameStatus.Status.Player3Turn :
                PlayerTurn == 4 ? GameStatus.Status.Player4Turn :
                PlayerTurn == 5 ? GameStatus.Status.Player5Turn :
                GameStatus.Status.Player6Turn;
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void UpdateFields(Action<Task> OnComplete,Dictionary<string,object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id,dict, OnComplete);
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
            PlayerNames[PlayerIndicator] = string.Empty;
            if (PlayerIndicator == 0)
                DeleteDocument(OnComplete);
            else
            {
                for (int i = 0; i < PlayerCount - 1; i++)
                {
                    if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                    {
                        PlayerNames[i] = PlayerNames[i + 1];
                        PlayerNames[i + 1] = string.Empty;
                    }
                }
                IsFull = false;
                PlayerLeft = PlayerIndicator;
                Dictionary<string, object> dict = new()
                {

                    { nameof(IsFull), IsFull },
                    { nameof(PlayerNames), PlayerNames },

                };
                UpdateFields(OnCompletePlayerLeft, dict);
            }
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id);
            fbd.DeleteDocument(Keys.GameCodesCollection,GameCode,OnComplete);
        }

        public override void EndTurn()
        {
            if (PlayerTurn == PlayerCount)
                PlayerTurn = 1;
            else
                PlayerTurn++;
            UpdateStatus();
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerTurn), PlayerTurn },
            };
            UpdateFields(OnTurnChanged, dict);
        }
        public override void StartGame()
        {
            GameStarted = true;
            Dictionary<string, object> dict = new()
            {
                { nameof(GameStarted), GameStarted },
            };
            UpdateFields(OnTurnChanged, dict);
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(this), true);
            });
        }

        private void OnCompletePlayerLeft(Task task)
        {
            OnPlayerLeft?.Invoke(this, PlayerIndicator);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                for (int i = 1; i < PlayerCount - 1; i++)
                {
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                        OnPlayerLeft?.Invoke(this, i);
                }
                IsFull = updatedGame.IsFull;
                PlayerNames = updatedGame.PlayerNames;
                PlayerTurn = updatedGame.PlayerTurn;
                if (updatedGame.GameStarted && !GameStarted)
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.Navigation.PushAsync(new GamePage(this), true);
                    });
                GameStarted = updatedGame.GameStarted;
                UpdateStatus();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
