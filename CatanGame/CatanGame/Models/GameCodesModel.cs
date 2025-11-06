using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public abstract class GameCodesModel
    {
        protected FbData fbd = new();
        public string GameId { get; set; } = string.Empty;
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void GetDocument(string GameCode, Action<Task> OnComplete);
    }
}
