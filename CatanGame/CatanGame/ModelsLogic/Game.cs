using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using System.Timers;
using static Xamarin.Io.OpenCensus.Stats.Aggregation;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        protected override GameStatus Status => _status;

        private static Button CreateRoadButton(int rotation)
        {
            return new()
            {
                Background = Colors.Transparent,
                BorderColor = Colors.White,
                BorderWidth = 1,
                Rotation = rotation,
                HeightRequest = 6,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 18
            };
        }
        private static Image CreateTileImage(string imageSource)
        {
            return new Image
            {
                Source = imageSource,
                HeightRequest = 79,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }
        private static Image CreateNumberImage(string imageSource)
        {
            return new Image
            {
                Source = imageSource,
                HeightRequest = 22,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }
        private static Button CreateApexButton()
        {
            return new()
            {
                Background = Colors.Transparent,
                BorderColor = Colors.White,
                BorderWidth = 1,
                HeightRequest = 10,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 10  
            };
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
                        for (int j = 1; j < PlayerCount; j++)
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

        public Game(GameSize slectedAmountOfPlayers,int selectedAmountOfPoints,int turnTime,bool isRandomBorad)
        {
            TurnTime = turnTime;
            ISRandomBoard = isRandomBorad;
            PlayerCount = slectedAmountOfPlayers.Size;
            AmountOfPointsNeeded = selectedAmountOfPoints;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            UpdateStatus();
        }
        public Game()
        {
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
        public override void Init(Grid gameBorad)
        {
            Grid board = [];
            board.RowDefinitions.Add(new RowDefinition { Height = new(0.8, GridUnitType.Star) });
            for (int i = 0; i < 5; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }
            board.RowDefinitions.Add(new RowDefinition { Height = new(0.86, GridUnitType.Star) });
            Grid Row = [];
            if (PlayerIndicator != 0)
            {
                for (int i = 1; i < 6; i++)
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < 4 + (i > 2 ? 5 - i : i - 1); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(0.78, GridUnitType.Star) });
                        Row.Add(CreateTileImage(TileTypes[(i - 1) * 5 + k - 1]), k);
                        Row.Add(CreateNumberImage(TileNumbers[(i - 1) * 5 + k - 1]), k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
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
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for(int k = 1; k < 4 + (i > 2 ? 5 - i : i - 1); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(0.78, GridUnitType.Star) });
                    }
                    for (int k = 1; k < 4 + (i > 2 ? 5 - i : i - 1); k++)
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
                            else if(i==2)
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
                            else if(i==3)
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
                            else if(i==4)
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
                                sourceNumber = String.Empty;
                                while (sourceNumber == String.Empty)
                                {
                                    curent = random.Next(0, numbers.Length - count);
                                    if (numbers[curent] != String.Empty)
                                    {
                                        sourceNumber = numbers[curent];
                                        numbers[curent] = String.Empty;
                                    }
                                }
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
                        TileTypes[(i - 1) * 5 + k - 1] = sourceTile;
                        Row.Add(CreateTileImage(sourceTile), k);
                        TileNumbers[(i - 1) * 5 + k - 1] = sourceNumber;
                        Row.Add(CreateNumberImage(sourceNumber), k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });            
                    board.Add(Row, 0, i);
                    Row = new()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                }
                //Dictionary<string, object> dict = new()
                //{
                //    {nameof(TileNumbers), TileNumbers },
                //    {nameof(TileTypes), TileTypes }
                //};
                //UpdateFields(dict);
            }
            //Grid picesLocations = [];
            //picesLocations.RowDefinitions.Add(new RowDefinition { Height = new(7.75, GridUnitType.Star) });
            //for (int i = 0; i < 11; i++)
            //{
            //    if(i % 2 != 0)
            //    {
            //        picesLocations.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            //        picesLocations.RowDefinitions.Add(new RowDefinition { Height = new(1.1, GridUnitType.Star) });
            //        picesLocations.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            //    }
            //    else
            //    {
            //        picesLocations.RowDefinitions.Add(new RowDefinition { Height = new(3.4, GridUnitType.Star) });
            //    }

            //}
            //picesLocations.RowDefinitions.Add(new RowDefinition { Height = new(7.75, GridUnitType.Star) });
            //for (int i = 1; i < 24; i++)
            //{
            //    if(i == 1 || i == 23)
            //    {
            //        Row.RowDefinitions.Add(new RowDefinition { Height = new(2, GridUnitType.Star) });
            //        for (int j = 1; j < 4; j++)
            //        {
            //            Row.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            //            Row.Add(CreateApexButton(),j,0);
            //        }
            //        Row.RowDefinitions.Add(new RowDefinition { Height = new(2, GridUnitType.Star) });
            //    }
            //    else if(i == 2)
            //    {
            //        Row.RowDefinitions.Add(new RowDefinition { Height = new(4, GridUnitType.Star) });
            //        for (int j = 1; j < 7; j++)
            //        {
            //            Row.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            //            Row.Add(CreateRoadButton(j % 2 != 0 ? -30 : 30), j, 0);
            //        }
            //        Row.RowDefinitions.Add(new RowDefinition { Height = new(4, GridUnitType.Star) });
            //    }
            //    else if(i == 22)
            //    {
            //        Row.RowDefinitions.Add(new RowDefinition { Height = new(4, GridUnitType.Star) });
            //        for (int j = 1; j < 7; j++)
            //        {
            //            Row.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            //            Row.Add(CreateRoadButton(j % 2 != 0 ? -30 : 30), j, 0);
            //        }
            //        Row.RowDefinitions.Add(new RowDefinition { Height = new(4, GridUnitType.Star) });
            //        Row.Rotation = 180;
            //    }
            //    picesLocations.Add(Row, 0, i);
            //}
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
    }
}
