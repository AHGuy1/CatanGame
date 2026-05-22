using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;

namespace CatanGame.Models
{
    public abstract class GameCodeModel
    {
        #region Fields
        protected FbData fbd = new();
        #endregion

        #region Properties
        public string GameId { get; set; } = string.Empty;
        public string GameCode { get; set; } = string.Empty;
        #endregion

        #region PublicMethods
        // Saves the game code document.
        public abstract void SetDocument(Action<Task> OnComplete);
        // Loads a game code document.
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
        #endregion

        #region Private Methods
        // Generates a six digit join code.
        protected static string RandomCodeGenerator() => string.Empty;
        #endregion
    }
}
