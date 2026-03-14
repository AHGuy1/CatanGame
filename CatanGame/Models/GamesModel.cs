using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace CatanGame.Models
{
    public abstract class GamesModel
    {
        #region Fields
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameCode? CurrentGameCode;
        #endregion

        #region Events
        public EventHandler<Game>? GameAdded;
        public EventHandler? GamesChanged;
        #endregion

        #region Properties
        public Game? CurrentGame;
        public bool IsBusy { get; set; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<GameSize>? AmountOfPlayers { get; set; } = [new GameSize(3), new GameSize(4), new GameSize(5), new GameSize(6)];
        public ObservableCollection<TurnTime> TurnTimes { get; set; } = [new TurnTime(20), new TurnTime(30), new TurnTime(45), new TurnTime(60), new TurnTime(75), new TurnTime(90), new TurnTime(120), new TurnTime(210), new TurnTime(300)];
        public static ObservableCollection<int> AmountOfPointsNeeded { get; set; } = [8, 9, 10, 11, 12, 13, 14, 15, 16];
        public static ObservableCollection<string> BoardTypes { get; set; } = [Strings.RandomBoardLabel, Strings.ClasicBoardLabel];
        #endregion

        #region PublicMethods
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void AddGame(GameSize slectedAmountOfPlayers, int selectedAmountOfPoints, int TurnTime, bool isRandomBoard);
        public abstract void JoinGameWithCode(string gameCode);
        #endregion

        #region PrivateMethods
        protected abstract void OnCompleteGameCodeAdded(Task task);
        protected abstract void OnCompleteGameAdded(Task task);
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        protected abstract void OnChange(IQuerySnapshot qs);
        protected abstract void OnCompleteGetCodeDocument(IDocumentSnapshot ds);
        protected abstract void OnCompleteGetGameDocument(IDocumentSnapshot ds);
        #endregion
    }
}
