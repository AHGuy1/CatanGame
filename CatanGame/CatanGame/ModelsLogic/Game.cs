using CatanGame.Models;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        public Game(GameSize selectedGameSize)
        {
            PlayerCount = selectedGameSize.Size;
            PlayerNames = new string[PlayerCount];
            PlayerNames[0] = fbd.DisplayName;
            Created = DateTime.Now;
        }
        public Game()
        {
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void GetDocument(string Id, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GamesCollection, Id , OnComplete);
        }
    }
}
