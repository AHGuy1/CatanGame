using CatanGame.Models;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class GameCode : GameCodeModel
    {
        #region Constructor
        public GameCode(string GameId)
        {
            this.GameId = GameId;
            GameCode = RandomCodeGenerator();
        }

        public GameCode()
        {
        }
        #endregion

        #region Public Methods
        public override void SetDocument(Action<Task> OnComplete)
        {
            fbd.SetDocument(this, Keys.GameCodesCollection, GameCode, OnComplete);
        }

        public override void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GameCodesCollection, GameCode, OnComplete);
        }
        #endregion

        #region Private Methods
        private static string RandomCodeGenerator()
        {
            Random random = new();
            return Convert.ToString(random.Next(100000, 999999));
        }
        #endregion
    }
}
