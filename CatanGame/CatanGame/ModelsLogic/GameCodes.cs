using CatanGame.Models;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class GameCodes : GameCodesModel
    {
        public GameCodes(string GameId)
        {
            this.GameId = GameId;
            this.GameCode = RandomCodeGenerator();
        }
        public GameCodes()
        {
        }

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

        //public override void GetDocumentsWhereEqualTo(string GameCode, Action<IQuerySnapshot> OnComplete)
        //{
        //    fbd.GetDocumentsWhereEqualTo(Keys.GameCodesCollection, "GameCode", GameCode, OnComplete);
        //}
    }
}
