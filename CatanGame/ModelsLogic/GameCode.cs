using CatanGame.Models;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class GameCode : GameCodeModel
    {
        private static string RandomCodeGenerator()
        {
            Random random = new();
            return Convert.ToString(random.Next(100000, 999999));
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            fbd.SetDocument(this, Keys.GameCodesCollection, GameCode, OnComplete);
        }
        public override void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GameCodesCollection, GameCode, OnComplete);
        }

        public GameCode(string GameId)
        {
            this.GameId = GameId;
            GameCode = RandomCodeGenerator();
        }
        public GameCode()
        {
        }
    }
}
