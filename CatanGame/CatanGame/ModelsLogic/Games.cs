using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Games : GamesModel
    {
        public void AddGame(GameSize slectedAmountOfPlayers,int selectedAmountOfPoints,int TurnTime)
        {
            IsBusy = true;
            if (selectedAmountOfPoints == 0)
                selectedAmountOfPoints = 10;
            if (TurnTime == 0)
                TurnTime = 60;
            Game game = new(slectedAmountOfPlayers,selectedAmountOfPoints, TurnTime);
            CurrentGame = game;
            game.SetDocument(OnCompleteGameAdded);

        }

        public void JoinGameWithCode(string gameCode)
        {
            IsBusy = true;
            GameCode gamecode = new();
            gamecode.GetDocument(gameCode,OnCompleteGetCodeDocument);
        }
        private void OnCompleteGameCodeAdded(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, CurrentGame!);
        }
        private void OnCompleteGameAdded(Task task)
        {
            GameCode gameCode = new(CurrentGame!.Id);
            CurrentGame.GameCode = gameCode.GameCode;
            gameCode.SetDocument(OnCompleteGameCodeAdded);

        }
        public Games()
        {

        }
        public void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        public void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }
        private void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnChange);
        }

        private void OnChange(IQuerySnapshot qs)
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
            OnGamesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnCompleteGetCodeDocument(IDocumentSnapshot ds)
        {
            if (ds.Data != null)
            {
                GameCode? gameCode = ds.ToObject<GameCode>();
                Game? game = new();
                game.GetDocument(gameCode!.GameId, OnCompleteGetGameDocument);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.GameDoesNotExiest, ToastDuration.Long, 20).Show();
                });
            } 
        }
        private void OnCompleteGetGameDocument(IDocumentSnapshot ds)
        {
            if (ds.Data != null)
            {
                Game? game = ds.ToObject<Game>();
                game!.Id = ds.Id;
                if (!game.IsFull)
                {
                    if (Application.Current != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Shell.Current.Navigation.PushAsync(new WaitingRoomPage(game), true);
                        });
                    }
                }
                else
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Toast.Make(Strings.GameIsFull, ToastDuration.Long, 20).Show();
                    });
                }
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.GameDoesNotExiest, ToastDuration.Long, 20).Show();
                });
            }

        }
    }
}
