using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Games : GamesModel
    {
        #region Constructor
        public Games()
        {
        }
        #endregion

        #region Private Methods
        // Finishes the add game flow after the join code is saved.
        protected override void OnCompleteGameCodeAdded(Task task)
        {
            IsBusy = false;
            GameAdded?.Invoke(this, CurrentGame!);
        }

        // Creates and saves a join code for a newly added game.
        protected override void OnCompleteGameAdded(Task task)
        {
            GameCode gameCode = new(CurrentGame!.Id);
            CurrentGame.GameCode = gameCode.GameCode;
            gameCode.SetDocument(OnCompleteGameCodeAdded);
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentGame.GameCode), gameCode.GameCode }
            };
            CurrentGame.UpdateFields(dict);
        }
        // Refreshes available games when the games collection changes.
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnChange);
        }

        // Rebuilds the local games list from a query snapshot.
        protected override void OnChange(IQuerySnapshot qs)
        {
            GamesList!.Clear();
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    game.Id = ds.Id;
                    GamesList.Add(game);
                }
            }
            GamesChanged?.Invoke(this, EventArgs.Empty);
        }

        // Loads the game referenced by a join code document.
        protected override void OnCompleteGetCodeDocument(IDocumentSnapshot ds)
        {
            if (ds.Data != null)
            {
                GameCode? gameCode = ds.ToObject<GameCode>();
                Game? game = new();
                game.GetDocument(gameCode!.GameId, OnCompleteGetGameDocument);
            }
            else
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.GameDoesNotExiest, ToastDuration.Long, 20).Show());
        }

        // Opens the waiting room for a game loaded by code.
        protected override void OnCompleteGetGameDocument(IDocumentSnapshot ds)
        {
            if (ds.Data != null)
            {
                Game? game = ds.ToObject<Game>();
                game!.Id = ds.Id;
                if (!game.IsFull)
                    if (Application.Current != null)
                        MainThread.BeginInvokeOnMainThread(() => Shell.Current.Navigation.PushAsync(new WaitingRoomPage(game), true));
                    else
                        MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.GameIsFull, ToastDuration.Long, 20).Show());
            }
            else
                MainThread.InvokeOnMainThreadAsync(() => Toast.Make(Strings.GameDoesNotExiest, ToastDuration.Long, 20).Show());
        }
        #endregion

        #region Public Methods
        // Starts listening for available game changes.
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }

        // Stops listening for available game changes.
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }

        // Creates a new game with the selected settings.
        public override void AddGame(GameSize slectedAmountOfPlayers, int selectedAmountOfPoints, int TurnTime, bool isRandomBoard)
        {
            IsBusy = true;
            if (selectedAmountOfPoints == 0)
                selectedAmountOfPoints = 10;
            if (TurnTime == 0)
                TurnTime = 60;
            Game game = new(slectedAmountOfPlayers, selectedAmountOfPoints, TurnTime, isRandomBoard);
            CurrentGame = game;
            game.SetDocument(OnCompleteGameAdded);
        }

        // Starts joining a game by its join code.
        public override void JoinGameWithCode(string gameCode)
        {
            IsBusy = true;
            GameCode gamecode = new();
            gamecode.GetDocument(gameCode, OnCompleteGetCodeDocument);
        }
        #endregion
    }
}
