using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;

namespace CatanGame.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        protected abstract GameStatus Status { get; }
        [Ignored]
        public System.Timers.Timer Timer = new();
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        [Ignored]
        public EventHandler? EndTurnOutOfTime;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler<int>? OnPlayerLeft;
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public int PlayerLeft { get; set; }
        [Ignored]
        public int PlayerIndicator { get; set; }
        [Ignored]
        public bool ISRandomBoard { get; set; }
        public int TurnTime { get; set; }
        public int AmountOfPointsNeeded { get; set; }
        public bool GameStarted { get; set; } = false;
        public int PlayerTurn { get; set; } = 1;
        public string GameCode { get; set; } = string.Empty;
        public string[] PlayerNames { get; set; } = [string.Empty];
        public string[] TileNumbers { get; set; } = new string[25];
        public string[] TileTypes { get; set; } = new string[25];
        public DateTime Created { get; set; }
        public int PlayerCount { get; set; }
        public bool IsFull { get; set; }
        protected abstract void UpdateStatus();
        public abstract void StartGame();
        public abstract void AddPlayerName();
        public abstract void EndTurn();
        public abstract void Init(Grid board);
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<Task> OnComplete);
        public abstract void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict);
        public abstract void UpdateFields(Dictionary<string, object> dict);

    }
}
