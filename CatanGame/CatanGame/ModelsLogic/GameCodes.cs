using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class GameCodes : GameCodesModel
    {
        public GameCodes(string GameId)
        {
            this.GameId = GameId;
        }
        public GameCodes()
        {
        }

        private string RandomCodeGenerator()
        {
            Random random = new Random();
            return Convert.ToString(random.Next(100000, 999999));
        }

        public override void SetDocument(Action<Task> OnComplete)
        {
            fbd.SetDocument(this, Keys.GameCodesCollection,RandomCodeGenerator(), OnComplete);
        }

        public override void GetDocument(string GameCode, Action<Task> OnComplete)
        {
            fbd.GetDocument(Keys.GameCodesCollection, GameCode, OnComplete);
        }
    }
}
