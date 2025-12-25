using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using System.Timers;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        public override GameStatus Status => _status;
        protected override void OnCompletePlayerLeft(Task task)
        {
            OnPlayerLeft?.Invoke(this, PlayerIndicator);
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                for (int i = 1; i < PlayerCount; i++)
                {
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                    {
                        for (int j = 1; j < PlayerCount; j++)
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
                if (TileTypes[0] == null)
                {
                    TileNumbers = updatedGame.TileNumbers;
                    TileTypes = updatedGame.TileTypes;
                }
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
        protected override void OnCompleteDeleted(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }
        protected override void OnCompleteAddPlayerName(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);
        }
        protected override void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void TurnTimerElapsed(object? sender, ElapsedEventArgs e)
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
            TimePassed = 0;
            OneSecondTimer.Stop();
            OneSecondTimer = new(1000);
            OneSecondTimer.Elapsed += OneSecondElapsed;
            OneSecondTimer.Start();
            if (PlayerTurn == PlayerIndicator + 1 && !Timer.Enabled && GameStarted)
            {
                Timer = new(TurnTime * 1000);
                Timer.Start();
                Timer.Elapsed += TurnTimerElapsed;
            }
            else if ((PlayerTurn != PlayerIndicator + 1 && Timer.Enabled) || !GameStarted)
                Timer.Stop();
        }

        public override void OneSecondElapsed(object? sender, ElapsedEventArgs e)
        {
            OneSecondTimer.Stop();
            TimePassed++;
            TimePssedUpdated?.Invoke(this, EventArgs.Empty);
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        public override void UpdateFields(Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict);
        }
        public override void GetDocument(string Id, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GamesCollection, Id, OnComplete);
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
            fbd.DeleteDocument(Keys.GameCodesCollection, GameCode, OnComplete);
        }
        public override void EndTurn()
        {
            Timer.Stop();
            if (PlayerTurn == PlayerCount)
                PlayerTurn = 1;
            else
                PlayerTurn++;
            Turn++;
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerTurn), PlayerTurn },
                { nameof(Turn), Turn }
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
        public Game(GameSize slectedAmountOfPlayers, int selectedAmountOfPoints, int turnTime, bool isRandomBoard)
        {
            TurnTime = turnTime;
            ISRandomBoard = isRandomBoard;
            PlayerCount = slectedAmountOfPlayers.Size;
            AmountOfPointsNeeded = selectedAmountOfPoints;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            UpdateStatus();
        }
        public Game()
        {
        }
    }
}
