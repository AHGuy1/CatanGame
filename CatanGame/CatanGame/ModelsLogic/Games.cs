using CatanGame.Models;
using CatanGame.Views;
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
            CurrentGame = game;
            CurrentGame.OnGameDeleted += OnGameDeleted;
            CurrentGame.OnPlayerLeft += OnPlayerLeft;
            game.SetDocument(OnCompleteGameAdded);

        }

        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted, ToastDuration.Long, 20).Show();
            });
        }
        private void OnPlayerLeft(object? sender, int Player)
        {
            if (Player == 1)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player2Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 2)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player3Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 3)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player4Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 4)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player5Left, ToastDuration.Long, 20).Show();
                });
            else if (Player == 5)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(Strings.Player6Left, ToastDuration.Long, 20).Show();
                });
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
