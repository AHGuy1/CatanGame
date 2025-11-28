using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using System.Timers;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        protected override GameStatus Status => _status;
        public Game(GameSize slectedAmountOfPlayers,int selectedAmountOfPoints,int turnTime)
        {
            TurnTime = turnTime;
            timer = new(TurnTime * 1000);
            PlayerCount = slectedAmountOfPlayers.Size;
            AmountOfPointsNeeded = selectedAmountOfPoints;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            timer.Elapsed += TurnTimerElapsed;
            UpdateStatus();
        }
        public Game()
        {
            if(TurnTime != 0)
            timer = new(TurnTime * 1000);
            timer.Elapsed += TurnTimerElapsed;
            UpdateStatus();
        }

        private void TurnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            EndTurnOutOfTime?.Invoke(this, EventArgs.Empty);
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
            if (PlayerTurn == PlayerIndicator + 1)
                timer.Start();
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
            if (PlayerIndicator == 0 || GameStarted)
                DeleteDocument(OnCompleteDeleted);
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
            timer.Stop();
            if (PlayerTurn == PlayerCount)
                PlayerTurn = 1;
            else
                PlayerTurn++;
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerTurn), PlayerTurn },
            };
            UpdateFields(OnTurnChanged, dict);
            OnEndedTurn?.Invoke(this, EventArgs.Empty);
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
                Application.Current!.MainPage = new GamePage(this);
            });
        }

        public override void AddPlayerName()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                {
                    PlayerNames[i] = fbd.DisplayName;
                    if (i + 1 == PlayerCount)
                        IsFull = true;
                    Dictionary<string, object> dict = new()
                    {

                        { nameof(IsFull), IsFull },
                        { nameof(PlayerNames), PlayerNames },

                    };
                    UpdateFields(OnCompleteAddPlayerName, dict);
                    PlayerIndicator = i;
                    i = PlayerCount;
                }
            }
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
                for (int i = 1; i < PlayerCount; i++)
                {
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                    {
                        for(int j = 1; j < PlayerCount; j++)
                        {
                            if (PlayerNames[j] != updatedGame.PlayerNames[j])
                            {
                                OnPlayerLeft?.Invoke(this, j);
                                if (j < PlayerIndicator)
                                    PlayerIndicator--;
                                j = PlayerCount;
                            }
                        }
                        i = PlayerCount;
                    }
                }
                IsFull = updatedGame.IsFull;
                PlayerNames = updatedGame.PlayerNames;
                PlayerTurn = updatedGame.PlayerTurn;
                TurnTime = updatedGame.TurnTime;
                timer = new(TurnTime);
                if (updatedGame.GameStarted && !GameStarted)
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        GameStarted = updatedGame.GameStarted;
                        Application.Current!.MainPage = new GamePage(this);
                    });
                UpdateStatus();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnCompleteDeleted(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnCompleteAddPlayerName(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);
        }

        private void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
