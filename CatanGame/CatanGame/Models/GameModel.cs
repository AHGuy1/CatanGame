using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CatanGame.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected abstract GameStatus Status { get; }
        protected GameStatus _status = new();
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
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
        public bool GameStarted { get; set; } = false;
        public int PlayerTurn { get; set; } = 1;
        public string GameCode { get; set; } = string.Empty;
        public string[] PlayerNames { get; set; } = [string.Empty];
        public DateTime Created { get; set; }
        public int PlayerCount { get; set; }
        public bool IsFull { get; set; }
        protected abstract void UpdateStatus();
        public abstract void StartGame();
        public abstract void EndTurn();
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<Task> OnComplete);
        public abstract void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict);
    }
}
