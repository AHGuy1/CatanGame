using CatanGame.ModelsLogic;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace CatanGame.Models
{
    public class GamesModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameCode? CurrentGameCode;
        public Game? CurrentGame;
        public bool IsBusy { get; set; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<GameSize>? AmountOfPlayers { get; set; } = [new GameSize(3), new GameSize(4), new GameSize(5), new GameSize(6)];
        public ObservableCollection<TurnTime> TurnTimes { get; set; } = [new TurnTime(20), new TurnTime(30), new TurnTime(45), new TurnTime(60), new TurnTime(75), new TurnTime(90), new TurnTime(120), new TurnTime(1), new TurnTime(300)];
        public static ObservableCollection<int> AmountOfPointsNeeded { get; set; } = [8, 9, 10, 11, 12, 13, 14, 15, 16];
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
    }
}
