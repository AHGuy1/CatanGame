using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;

namespace CatanGame.Models
{
    public abstract class GameCodeModel
    {
        protected FbData fbd = new();
        public string GameId { get; set; } = string.Empty;
        public string GameCode { get; set; } = string.Empty;
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
    }
}
