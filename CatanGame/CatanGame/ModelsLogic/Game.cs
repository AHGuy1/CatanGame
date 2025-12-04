using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using System.Timers;
using static Xamarin.Io.OpenCensus.Stats.Aggregation;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        protected override GameStatus Status => _status;
        public Game(GameSize slectedAmountOfPlayers,int selectedAmountOfPoints,int turnTime)
        {
            TurnTime = turnTime;
            PlayerCount = slectedAmountOfPlayers.Size;
            AmountOfPointsNeeded = selectedAmountOfPoints;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            UpdateStatus();
        }
        public Game()
        {
        }

        private void TurnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            EndTurnOutOfTime?.Invoke(this, EventArgs.Empty);
        }

        protected override void UpdateStatus()
        {
            _status.CurrentStatus = !GameStarted ? GameStatus.Status.PleseWait :
                PlayerTurn == PlayerIndicator + 1 ? GameStatus.Status.YourTurn :
                PlayerTurn == 1 ? GameStatus.Status.Player1Turn :
                PlayerTurn == 2 ? GameStatus.Status.Player2Turn :
                PlayerTurn == 3 ? GameStatus.Status.Player3Turn :
                PlayerTurn == 4 ? GameStatus.Status.Player4Turn :
                PlayerTurn == 5 ? GameStatus.Status.Player5Turn :
                GameStatus.Status.Player6Turn;
            if (PlayerTurn == PlayerIndicator + 1 && !Timer.Enabled && GameStarted)
            {
                Timer = new(TurnTime * 1000);
                Timer.Start();
                Timer.Elapsed += TurnTimerElapsed;
            }
            else if ((PlayerTurn != PlayerIndicator + 1 && Timer.Enabled) || !GameStarted) 
                Timer.Stop();
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void UpdateFields(Action<Task> OnComplete,Dictionary<string,object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id,dict, OnComplete);
        }
        public override void UpdateFields(Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict);
        }
        public override void GetDocument(string Id, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GamesCollection, Id , OnComplete);
        }
        public override void Init(Grid board)
        {
                GridLength gridLength = new(0.8, GridUnitType.Star);
                board.RowDefinitions.Add(new RowDefinition { Height = gridLength });
                for (int i = 0; i < 5; i++)
                {
                    board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                }
                gridLength = new(0.86, GridUnitType.Star);
                board.RowDefinitions.Add(new RowDefinition { Height = gridLength });
                Grid Row = [];
                Image image;

            if (PlayerIndicator != 0)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i == 1 || i == 5)
                    {
                        gridLength = new(0.78, GridUnitType.Star);
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int j = 1; j < 4; j++)
                        {
                            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = gridLength });
                        }
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int k = 1; k < 4; k++)
                        {
                            image = new()
                            {
                                Source = TileTypes[(i-1)*5 + k-1],
                                HeightRequest = 79,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            Row.Add(image, k);
                            image = new()
                            {
                                Source = TileNumbers[(i - 1) * 5 + k - 1],
                                HeightRequest = 22,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            Row.Add(image, k);
                        }
                    }
                    else if (i == 2 || i == 4)
                    {
                        gridLength = new(0.78, GridUnitType.Star);
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int j = 1; j < 5; j++)
                        {
                            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = gridLength });
                        }
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int k = 1; k < 5; k++)
                        {
                            image = new()
                            {
                                Source = TileTypes[(i - 1) * 5 + k - 1],
                                HeightRequest = 79,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            Row.Add(image, k);
                            image = new()
                            {
                                Source = TileNumbers[(i - 1) * 5 + k - 1],
                                HeightRequest = 22,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            Row.Add(image, k);
                        }
                    }
                    else
                    {
                        gridLength = new(0.79, GridUnitType.Star);
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int j = 1; j < 6; j++)
                        {
                            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = gridLength });
                        }
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int k = 1; k < 6; k++)
                        {
                            image = new()
                            {
                                Source = TileTypes[(i - 1) * 5 + k - 1],
                                HeightRequest = 79,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            Row.Add(image, k);
                            image = new()
                            {
                                Source = TileNumbers[(i - 1) * 5 + k - 1],
                                HeightRequest = 22,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            Row.Add(image, k);
                        }
                    }
                    board.Add(Row, 0, i);
                    Row = new()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                }
            }
            else
            {
                string sourceTile;
                string sourceNumber;
                Random random = new();
                int count = 0;
                string[] tiles =
                [
                    Strings.FieldsTwo,Strings.FieldsTwo,Strings.FieldsOne,Strings.FieldsOne,
                Strings.Mountien,Strings.Mountien,Strings.Mountien,
                Strings.Hill,Strings.Hill,Strings.Hill,
                Strings.Forest,Strings.Forest, Strings.Forest,Strings.Forest,
                Strings.Pasture,Strings.Pasture,Strings.Pasture,Strings.Pasture,
                Strings.Desert
                ];
                string[] numbers =
                [
                    Strings.TwoImage,Strings.ThreeImage,Strings.ThreeImage,Strings.FourImage,
                Strings.FourImage,Strings.FiveImage,Strings.FiveImage,Strings.SixImage,
                Strings.SixImage,Strings.EightImage,Strings.EightImage,Strings.NineImage,
                Strings.NineImage,Strings.TenImage,Strings.TenImage,Strings.ElevenImage,
                Strings.ElevenImage,Strings.TwelveImage,
                String.Empty
                ];
                for (int i = 1; i < 6; i++)
                {
                    if (i == 1 || i == 5)
                    {
                        gridLength = new(0.78, GridUnitType.Star);
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int j = 1; j < 4; j++)
                        {
                            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = gridLength });
                        }
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int k = 1; k < 4; k++)
                        {
                            if (!ISRandomBoard)
                            {
                                if (i == 1)
                                {
                                    if (k == 1)
                                    {
                                        sourceTile = Strings.Mountien;
                                        sourceNumber = Strings.TenImage;
                                    }
                                    else if (k == 2)
                                    {
                                        sourceTile = Strings.Pasture;
                                        sourceNumber = Strings.TwoImage;
                                    }
                                    else
                                    {
                                        sourceTile = Strings.Forest;
                                        sourceNumber = Strings.NineImage;
                                    }
                                }
                                else
                                {
                                    if (k == 1)
                                    {
                                        sourceTile = Strings.Hill;
                                        sourceNumber = Strings.FiveImage;
                                    }
                                    else if (k == 2)
                                    {
                                        sourceTile = Strings.FieldsTwo;
                                        sourceNumber = Strings.SixImage;
                                    }
                                    else
                                    {
                                        sourceTile = Strings.Pasture;
                                        sourceNumber = Strings.ElevenImage;
                                    }
                                }
                            }
                            else
                            {
                                int curent = random.Next(0, tiles.Length - count);
                                sourceTile = tiles[curent];
                                tiles[curent] = String.Empty;
                                if (sourceTile == Strings.Desert)
                                {
                                    sourceNumber = numbers[numbers.Length - 1 - count];
                                    numbers[numbers.Length - 1 - count] = String.Empty;
                                }
                                else
                                {
                                    curent = random.Next(0, numbers.Length - count);
                                    sourceNumber = numbers[curent];
                                    numbers[curent] = String.Empty;
                                }
                                count++;
                                for (int n = 0; n < tiles.Length - 1; n++)
                                {
                                    if (tiles[n] == String.Empty)
                                    {
                                        tiles[n] = tiles[n + 1];
                                        tiles[n + 1] = String.Empty;
                                    }
                                    if (numbers[n] == String.Empty)
                                    {
                                        numbers[n] = numbers[n + 1];
                                        numbers[n + 1] = String.Empty;
                                    }
                                }
                            }
                            image = new()
                            {
                                Source = sourceTile,
                                HeightRequest = 79,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            TileTypes[(i - 1) * 5 + k - 1] = sourceTile;
                            Row.Add(image, k);
                            image = new()
                            {
                                Source = sourceNumber,
                                HeightRequest = 22,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            TileNumbers[(i - 1) * 5 + k - 1] = sourceNumber;
                            Row.Add(image, k);
                        }
                    }
                    else if (i == 2 || i == 4)
                    {
                        gridLength = new(0.78, GridUnitType.Star);
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int j = 1; j < 5; j++)
                        {
                            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = gridLength });
                        }
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int k = 1; k < 5; k++)
                        {
                            if (!ISRandomBoard)
                            {
                                if (i == 2)
                                {
                                    if (k == 1)
                                    {
                                        sourceTile = Strings.FieldsOne;
                                        sourceNumber = Strings.TwelveImage;
                                    }
                                    else if (k == 2)
                                    {
                                        sourceTile = Strings.Hill;
                                        sourceNumber = Strings.SixImage;
                                    }
                                    else if (k == 3)
                                    {
                                        sourceTile = Strings.Pasture;
                                        sourceNumber = Strings.FourImage;
                                    }
                                    else
                                    {
                                        sourceTile = Strings.Hill;
                                        sourceNumber = Strings.TenImage;
                                    }
                                }
                                else
                                {
                                    if (k == 1)
                                    {
                                        sourceTile = Strings.Forest;
                                        sourceNumber = Strings.EightImage;
                                    }
                                    else if (k == 2)
                                    {
                                        sourceTile = Strings.Mountien;
                                        sourceNumber = Strings.ThreeImage;
                                    }
                                    else if (k == 3)
                                    {
                                        sourceTile = Strings.FieldsTwo;
                                        sourceNumber = Strings.FourImage;
                                    }
                                    else
                                    {
                                        sourceTile = Strings.Pasture;
                                        sourceNumber = Strings.FiveImage;
                                    }
                                }
                            }
                            else
                            {
                                int curent = random.Next(0, tiles.Length - count);
                                sourceTile = tiles[curent];
                                tiles[curent] = String.Empty;
                                if (sourceTile == Strings.Desert)
                                {
                                    sourceNumber = numbers[numbers.Length - 1 - count];
                                    numbers[numbers.Length - 1 - count] = String.Empty;
                                }
                                else
                                {
                                    curent = random.Next(0, numbers.Length - count);
                                    sourceNumber = numbers[curent];
                                    numbers[curent] = String.Empty;
                                }
                                count++;
                                for (int n = 0; n < tiles.Length - 1; n++)
                                {
                                    if (tiles[n] == String.Empty)
                                    {
                                        tiles[n] = tiles[n + 1];
                                        tiles[n + 1] = String.Empty;
                                    }
                                    if (numbers[n] == String.Empty)
                                    {
                                        numbers[n] = numbers[n + 1];
                                        numbers[n + 1] = String.Empty;
                                    }
                                }
                            }
                            image = new()
                            {
                                Source = sourceTile,
                                HeightRequest = 79,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            TileTypes[(i - 1) * 5 + k - 1] = sourceTile;
                            Row.Add(image, k);
                            image = new()
                            {
                                Source = sourceNumber,
                                HeightRequest = 22,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            TileNumbers[(i - 1) * 5 + k - 1] = sourceNumber;
                            Row.Add(image, k);
                        }
                    }
                    else
                    {
                        gridLength = new(0.79, GridUnitType.Star);
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int j = 1; j < 6; j++)
                        {
                            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = gridLength });
                        }
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        for (int k = 1; k < 6; k++)
                        {
                            if (!ISRandomBoard)
                            {
                                if (k == 1)
                                {
                                    sourceTile = Strings.FieldsOne;
                                    sourceNumber = Strings.NineImage;
                                }
                                else if (k == 2)
                                {
                                    sourceTile = Strings.Forest;
                                    sourceNumber = Strings.ElevenImage;
                                }
                                else if (k == 3)
                                {
                                    sourceTile = Strings.Desert;
                                    sourceNumber = String.Empty;
                                }
                                else if (k == 4)
                                {
                                    sourceTile = Strings.Forest;
                                    sourceNumber = Strings.ThreeImage;
                                }
                                else
                                {
                                    sourceTile = Strings.Mountien;
                                    sourceNumber = Strings.EightImage;
                                }
                            }
                            else
                            {
                                int curent = random.Next(0, tiles.Length - count);
                                sourceTile = tiles[curent];
                                tiles[curent] = String.Empty;
                                if (sourceTile == Strings.Desert)
                                {
                                    sourceNumber = numbers[numbers.Length - 1 - count];
                                    numbers[numbers.Length - 1 - count] = String.Empty;
                                }
                                else
                                {
                                    curent = random.Next(0, numbers.Length - count);
                                    sourceNumber = numbers[curent];
                                    numbers[curent] = String.Empty;
                                }
                                count++;
                                for (int n = 0; n < tiles.Length - 1; n++)
                                {
                                    if (tiles[n] == String.Empty)
                                    {
                                        tiles[n] = tiles[n + 1];
                                        tiles[n + 1] = String.Empty;
                                    }
                                    if (numbers[n] == String.Empty)
                                    {
                                        numbers[n] = numbers[n + 1];
                                        numbers[n + 1] = String.Empty;
                                    }
                                }
                            }
                            image = new()
                            {
                                Source = sourceTile,
                                HeightRequest = 79,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            TileTypes[(i - 1) * 5 + k - 1] = sourceTile;
                            Row.Add(image, k);
                            image = new()
                            {
                                Source = sourceNumber,
                                HeightRequest = 22,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            TileNumbers[(i - 1) * 5 + k - 1] = sourceNumber;
                            Row.Add(image, k);
                        }
                    }
                    board.Add(Row, 0, i);
                    Row = new()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                }
                Dictionary<string, object> dict = new()
                {
                    {nameof(TileNumbers), TileNumbers },
                    {nameof(TileTypes), TileTypes }
                };
                UpdateFields(dict);
            }            
        }

        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            PlayerNames[PlayerIndicator] = string.Empty;
            if (PlayerIndicator == 0 || GameStarted)
                DeleteDocument(OnCompleteDeleted);
            else
            {
                for (int i = 0; i < PlayerCount - 1; i++)
                {
                    if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                    {
                        PlayerNames[i] = PlayerNames[i + 1];
                        PlayerNames[i + 1] = string.Empty;
                    }
                }
                IsFull = false;
                PlayerLeft = PlayerIndicator;
                Dictionary<string, object> dict = new()
                {

                    { nameof(IsFull), IsFull },
                    { nameof(PlayerNames), PlayerNames },

                };
                UpdateFields(OnCompletePlayerLeft, dict);
            }
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id);
            fbd.DeleteDocument(Keys.GameCodesCollection,GameCode,OnComplete);
        }

        public override void EndTurn()
        {
            Timer.Stop();
            if (PlayerTurn == PlayerCount)
                PlayerTurn = 1;
            else
                PlayerTurn++;
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerTurn), PlayerTurn },
            };
            UpdateFields(OnTurnChanged, dict);
        }
        public override void StartGame()
        {    
            GameStarted = true;
            Dictionary<string, object> dict = new()
            {
                { nameof(GameStarted), GameStarted },
            };
            UpdateFields(OnTurnChanged, dict);
        }

        public override void AddPlayerName()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                {
                    PlayerNames[i] = fbd.DisplayName;
                    if (i + 1 == PlayerCount)
                        IsFull = true;
                    Dictionary<string, object> dict = new()
                    {

                        { nameof(IsFull), IsFull },
                        { nameof(PlayerNames), PlayerNames },

                    };
                    UpdateFields(OnCompleteAddPlayerName, dict);
                    PlayerIndicator = i;
                    i = PlayerCount;
                }
            }
        }

        private void OnCompletePlayerLeft(Task task)
        {
            OnPlayerLeft?.Invoke(this, PlayerIndicator);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                for (int i = 1; i < PlayerCount; i++)
                {
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                    {
                        for(int j = 1; j < PlayerCount; j++)
                        {
                            if (PlayerNames[j] != updatedGame.PlayerNames[j])
                            {
                                OnPlayerLeft?.Invoke(this, j);
                                if (j < PlayerIndicator)
                                    PlayerIndicator--;
                                j = PlayerCount;
                            }
                        }
                        i = PlayerCount;
                    }
                }
                IsFull = updatedGame.IsFull;
                PlayerNames = updatedGame.PlayerNames;
                PlayerTurn = updatedGame.PlayerTurn;
                TurnTime = updatedGame.TurnTime;
                if (TileTypes[0] == null)
                {
                    TileNumbers = updatedGame.TileNumbers;
                    TileTypes = updatedGame.TileTypes; 
                }
                if (updatedGame.GameStarted && !GameStarted)
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        GameStarted = updatedGame.GameStarted;
                        Application.Current!.MainPage = new GamePage(this);
                    });
                UpdateStatus();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnCompleteDeleted(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnCompleteAddPlayerName(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);
        }

        private void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
