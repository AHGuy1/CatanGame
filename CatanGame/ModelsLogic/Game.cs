using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        public override GameStatus Status => _status;

        public Game(GameSize slectedAmountOfPlayers, int selectedAmountOfPoints, int turnTime, bool isRandomBoard)
        {
            TurnTime = turnTime;
            IsRandomBoard = isRandomBoard;
            PlayerCount = slectedAmountOfPlayers.Size;
            AmountOfPointsNeeded = selectedAmountOfPoints;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            UpdateStatus();
            IntArrayBoardPices();
        }
        public Game()
        {
            IntArrayBoardPices();
        }

        protected override void IntArrayBoardPices()
        {
            for (int i = 0; i < 276; i++)
                BoardPeices[i] = string.Empty;       
        }
        protected override void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }
        protected override void OnMessageReceived(long timeleft)
        {
           
            if (timeleft == Keys.FinishedSignal)
            {
                TimeLeft = Strings.TimeUp;
                EndTurnOutOfTime?.Invoke(this, EventArgs.Empty);
            }
            else if((double)timeleft/1000 <= 10.0)
            {
                TimeLeft = double.Round(((double)timeleft / 1000), 1).ToString();
                TimeLeftChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TimeLeft = double.Round(((double)timeleft / 1000), 0).ToString();
                TimeLeftChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        protected override void StartTimer()
        {
            TimerSettings ts = new((TurnTime * 1000) + 1, 10);
            WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(ts));
        }
        protected override void StopTimer()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<string>(Keys.StopSignal));
        }
        protected override void OnCompletePlayerLeft(Task task)
        {
            PlayerLeft?.Invoke(this, PlayerIndicator);
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                for (int i = 1; i < PlayerCount; i++)
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                    {
                        for (int j = 1; j < PlayerCount; j++)
                            if (PlayerNames[j] != updatedGame.PlayerNames[j])
                            {
                                PlayerLeft?.Invoke(this, j);
                                if (j < PlayerIndicator)
                                    PlayerIndicator--;
                                j = PlayerCount;
                            }
                        i = PlayerCount;
                    }
                IsFull = updatedGame.IsFull;
                PlayerNames = updatedGame.PlayerNames;
                TurnTime = updatedGame.TurnTime;
                if (TileTypes[0] == null)
                {
                    TileNumbers = updatedGame.TileNumbers;
                    TileTypes = updatedGame.TileTypes;
                }
                if(PlayerTurn != updatedGame.PlayerTurn)
                {
                    PlayerTurn = updatedGame.PlayerTurn;
                    Turn = updatedGame.Turn;
                    TurnChanged?.Invoke(this, EventArgs.Empty);
                    StartTimer();
                }
                bool gridChanged = false;
                for (int i = 0; i < BoardPeices.Length; i++)
                    if(BoardPeices[i] != updatedGame.BoardPeices[i])
                    {
                        gridChanged = true;
                        BoardPeices[i] = updatedGame.BoardPeices[i];
                    }
                if(LongestRoadLength != updatedGame.LongestRoadLength || LargestArmySize != updatedGame.LargestArmySize)
                {
                    LongestRoadLength = updatedGame.LongestRoadLength;
                    LargestArmySize = updatedGame.LargestArmySize;
                    gridChanged = true;
                }
                if (gridChanged)
                    GridChanged?.Invoke(this, EventArgs.Empty);
                if (updatedGame.GameStarted && !GameStarted)
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        RegisterTimer();
                        StartTimer();
                        GameStarted = updatedGame.GameStarted;
                        Application.Current!.MainPage = new GamePage(this);
                    });
                UpdateStatus();
                GameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if(!GameStarted)
                    GameDeleted?.Invoke(this,Strings.HostLeft);
                else
                    GameDeleted?.Invoke(this, string.Empty);
            }
        }
        protected override void OnCompleteDeleted(Task task)
        {
            if (task.IsCompletedSuccessfully)
                GameDeleted?.Invoke(this, string.Empty);
        }
        protected override void OnCompleteAddPlayerName(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);
        }
        protected override void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                GameChanged?.Invoke(this, EventArgs.Empty);
        }
        protected override void UpdateStatus()
        {
           Array status = Enum.GetValues(typeof(GameStatus.Status));
            _status.CurrentStatus = !GameStarted ? GameStatus.Status.PleseWait :
                PlayerTurn == PlayerIndicator + 1 ? GameStatus.Status.YourTurn :
                (GameStatus.Status)status.GetValue(PlayerTurn - 1)!;
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
            StopTimer();
            ilr?.Remove();                           
            PlayerNames[PlayerIndicator] = string.Empty;
            if (PlayerIndicator == 0 || GameStarted)
                DeleteDocument(OnCompleteDeleted);
            else
            {
                for (int i = 0; i < PlayerCount - 1; i++)
                    if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                    {
                        PlayerNames[i] = PlayerNames[i + 1];
                        PlayerNames[i + 1] = string.Empty;
                    }
                IsFull = false;
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
            StartTimer();
        }
        public override void StartGame()
        {
            if (!GameStarted)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    RegisterTimer();
                    StartTimer();
                });
                GameStarted = true;
                Dictionary<string, object> dict = new()
                {
                    { nameof(GameStarted), GameStarted },
                };
                UpdateFields(OnTurnChanged, dict);
            }       
        }
        public override void AddPlayerName()
        {
            bool addedName = false;
            for (int i = 0; i < PlayerCount && !addedName; i++)
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
                    addedName = true;
                }
        }
    }
}
