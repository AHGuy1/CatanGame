using CatanGame.Models;
using CatanGame.ModelsLogic;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        public Game(GameSize selectedGameSize)
        {
            HostName = User.LogedInUserName;
            PlayerCount = selectedGameSize.Size;
            Created = DateTime.Now;
        }
        public Game()
        {
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
            GameCodes gamecode = new(Id);
        }
    }
}
