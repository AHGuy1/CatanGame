using CatanGame.Models;
using Firebase.Auth.Requests;
using System.Windows.Input;
using CatanGame.ModelsLogic;

namespace CatanGame.ViewModels
{
    internal class HomePageVM
    {
        private readonly Games games = new();
        public static IList<GameSize> GameSizes => [new GameSize(3), new GameSize(4), new GameSize(5)];
        public GameSize SelectedGameSize { get; set; } = GameSizes[0];
        public ICommand AddGameComand => new Command(AddGame);
        public string DisplayName { get; set; } = GameSizes[0].DisplayName;
        private void AddGame()
        {

        }

        public IList<Game>? GamesList => games.GamesList;
    }
}
