using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CatanGame.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        [Ignored]
        public string Id { get; set; } = string.Empty;
        public string GameCode { get; set; } = string.Empty;
        public string[] PlayerNames { get; set; } = [string.Empty];
        public DateTime Created { get; set; }
        public int PlayerCount { get; set; }
        public bool IsFull { get; set; }
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);

    }
}
