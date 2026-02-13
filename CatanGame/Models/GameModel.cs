using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Timers;

namespace CatanGame.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        [Ignored]
        public abstract GameStatus Status { get; }
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        [Ignored]
        //= new()?
        public Board GameBoard { get; } = new();
        [Ignored]
        public EventHandler? TimeLeftChanged;
        [Ignored]
        public EventHandler? EndTurnOutOfTime;
        [Ignored]
        public EventHandler? GameChanged;
        [Ignored]
        public EventHandler? GridChanged;
        [Ignored]
        public EventHandler? TurnChanged;
        [Ignored]
        public EventHandler<string>? GameDeleted;
        [Ignored]
        public EventHandler<int>? PlayerLeft;
        [Ignored]
        public int PlayerLongestRoadLength { get; set; }
        [Ignored]
        public int PlayerLargestArmySize { get; set; }
        [Ignored]
        public int PlayerIndicator { get; set; }
        [Ignored]
        public int RollTotal => Roll1 + Roll2;
        [Ignored]
        public bool IsRandomBoard { get; set; }
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        public int Roll1 { get; set;}
        public int Roll2 { get; set; }
        public int LongestRoadLength { get; set; } = 4;
        public int LargestArmySize { get; set; } = 2;
        public int TurnTime { get; set; }
        public int Turn { get; set; } = 1;
        public int AmountOfPointsNeeded { get; set; }
        public bool GameStarted { get; set; }
        public int PlayerTurn { get; set; } = 1;
        public string GameCode { get; set; } = string.Empty;
        public string[] PlayerNames { get; set; } = [string.Empty];
        public string[] TileNumbers { get; set; } = new string[19];
        public string[] TileTypes { get; set; } = new string[19];
        public string[] BoardPieces { get; set; } = new string[276];
        public DateTime Created { get; set; }
        public int PlayerCount { get; set; }
        public bool IsFull { get; set; }
        protected abstract void UpdateStatus();
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        protected abstract void OnCompletePlayerLeft(Task task);
        protected abstract void OnCompleteDeleted(Task task);
        protected abstract void OnCompleteAddPlayerName(Task task);
        protected abstract void OnTurnChanged(Task task);
        protected abstract void StartTimer();
        protected abstract void RegisterTimer();
        protected abstract void StopTimer();
        protected abstract void OnMessageReceived(long timeleft);
        protected abstract void IntArrayBoardPieces();
        public abstract void StartGame();
        public abstract void AddPlayerName();
        public abstract void EndTurn();
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<Task> OnComplete);
        public abstract void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict);
        public abstract void UpdateFields(Dictionary<string, object> dict);
    }
}
