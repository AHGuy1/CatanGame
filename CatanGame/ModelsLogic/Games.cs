using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Games : GamesModel
    {
        public Games()
        {
        }

        protected override void OnCompleteGameCodeAdded(Task task)
        {
            IsBusy = false;
            GameAdded?.Invoke(this, CurrentGame!);
        }
        protected override void OnCompleteGameAdded(Task task)
        {
            GameCode gameCode = new(CurrentGame!.Id);
            CurrentGame.GameCode = gameCode.GameCode;
            gameCode.SetDocument(OnCompleteGameCodeAdded);
            Dictionary<string, object> dict = new()
            {
                {nameof(CurrentGame.GameCode),gameCode.GameCode }
            };
            CurrentGame.UpdateFields(dict);
        }
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnChange);
        }
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
        protected override void OnCompleteGetCodeDocument(IDocumentSnapshot ds)
        {
            if (ds.Data != null)
            {
                GameCode? gameCode = ds.ToObject<GameCode>();
                Game? game = new();
                game.GetDocument(gameCode!.GameId, OnCompleteGetGameDocument);
            }
            else
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.GameDoesNotExiest, ToastDuration.Long, 20).Show();
                });
        }
        protected override void OnCompleteGetGameDocument(IDocumentSnapshot ds)
        {
            if (ds.Data != null)
            {
                Game? game = ds.ToObject<Game>();
                game!.Id = ds.Id;
                if (!game.IsFull)
                    if (Application.Current != null)
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Shell.Current.Navigation.PushAsync(new WaitingRoomPage(game), true);
                        });
                else
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Toast.Make(Strings.GameIsFull, ToastDuration.Long, 20).Show();
                    });
            }
            else
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.GameDoesNotExiest, ToastDuration.Long, 20).Show();
                });
        }

        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }
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
        public override void JoinGameWithCode(string gameCode)
        {
            IsBusy = true;
            GameCode gamecode = new();
            gamecode.GetDocument(gameCode, OnCompleteGetCodeDocument);
        }
    }
}
