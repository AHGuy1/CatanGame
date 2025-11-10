using CatanGame.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Games : GamesModel
    {
        public void AddGame(GameSize selectedGameSize)
        {
            IsBusy = true;
            Game game = new(selectedGameSize);
            game.SetDocument(OnComplete);
            GameCodes gameCode = new(game.Id);
            gameCode.SetDocument(OnComplete);
        }
        public void JoinGameWithCode(string gameCode)
        {
            IsBusy = true;
            GameCodes gamecode = new();
            gamecode.GetDocument(gameCode,OnCompleteGetCodeDocument);
        }

        private void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
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
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnCompleteAddGame);
        }

        private void OnCompleteAddGame(IQuerySnapshot qs)
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
                GameCodes? gameCode = ds.ToObject<GameCodes>();
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
                            Shell.Current.Navigation.PushAsync(new GamePage(value), true);
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
