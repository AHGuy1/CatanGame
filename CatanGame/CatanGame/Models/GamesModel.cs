using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace CatanGame.Models
{
    public class GamesModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        public bool IsBusy { get; set; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<GameSize>? GameSizes { get; set; } = [new GameSize(3), new GameSize(4), new GameSize(5)];
        public EventHandler<bool>? OnGameAdded;
        public EventHandler? OnGamesChanged;
    }
}
