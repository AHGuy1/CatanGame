using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class GameGrid : GameGridModel
    {
        private static Grid CreateTileImage(string imageSource)
        {
            Grid grid = [];
            Image image = new()
            {
                Source = imageSource,
                HeightRequest = 71,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Button button = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 50,
                WidthRequest = 50,
                BackgroundColor = Colors.Transparent,
                IsEnabled = false
            };
            grid.Add(button);
            grid.Add(image);
            return grid;
        }
        private static Image CreateNumberImage(string imageSource)
        {
            return new()
            {
                Source = imageSource,
                HeightRequest = 22,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }

        protected override IndexedButton CreateRoadButton(int rotation, int colmnIndex, int rowIndex)
        {
            IndexedButton indexedButton = new(rowIndex, colmnIndex, 6, 18, rotation);
            BoardPiceButtons[rowIndex, colmnIndex] = indexedButton;
            return indexedButton;
        }
        protected override IndexedButton CreateApexButton(int colmnIndex, int rowIndex)
        {
            IndexedButton indexedButton = new(rowIndex, colmnIndex, 10, 10);
            BoardPiceButtons[rowIndex, colmnIndex] = indexedButton;
            return indexedButton;
        }
        public override void Init(Grid gameBoard, Grid grdPices, Game game)
        {
            gameBoard.RowDefinitions.Add(new RowDefinition { Height = new(0, GridUnitType.Star) });
            for (int i = 0; i < 5; i++)
            {
                gameBoard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }
            gameBoard.RowDefinitions.Add(new RowDefinition { Height = new(1.75, GridUnitType.Star) });
            gameBoard.RowSpacing = 0;
            Grid Row = new()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            if (game.PlayerIndicator != 0)
            {
                for (int i = 1; i < 6; i++)
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < 4 + (i > 2 ? 5 - i : i - 1); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(2, GridUnitType.Star) });
                        Row.Add(CreateTileImage(game.TileTypes[(i - 1) * 5 + k - 1]), k);
                        Row.Add(CreateNumberImage(game.TileNumbers[(i - 1) * 5 + k - 1]), k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    gameBoard.Add(Row, 0, i);
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
                Strings.MountienOne,Strings.MountienTwo,Strings.MountienOne,
                Strings.Hills,Strings.Hills,Strings.Hills,
                Strings.ForestTwo,Strings.ForestTwo, Strings.ForestOne,Strings.ForestOne,
                Strings.PastureTwo,Strings.PastureTwo,Strings.PastureOne,Strings.PastureOne,
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
                    for (int k = 1; k < 4 + (i > 2 ? 5 - i : i - 1); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(2, GridUnitType.Star) });
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < 4 + (i > 2 ? 5 - i : i - 1); k++)
                    {
                        if (!game.ISRandomBoard)
                        {
                            if (i == 1)
                            {
                                if (k == 1)
                                {
                                    sourceTile = Strings.MountienOne;
                                    sourceNumber = Strings.TenImage;
                                }
                                else if (k == 2)
                                {
                                    sourceTile = Strings.PastureOne;
                                    sourceNumber = Strings.TwoImage;
                                }
                                else
                                {
                                    sourceTile = Strings.ForestOne;
                                    sourceNumber = Strings.NineImage;
                                }
                            }
                            else if (i == 2)
                            {
                                if (k == 1)
                                {
                                    sourceTile = Strings.FieldsOne;
                                    sourceNumber = Strings.TwelveImage;
                                }
                                else if (k == 2)
                                {
                                    sourceTile = Strings.Hills;
                                    sourceNumber = Strings.SixImage;
                                }
                                else if (k == 3)
                                {
                                    sourceTile = Strings.PastureTwo;
                                    sourceNumber = Strings.FourImage;
                                }
                                else
                                {
                                    sourceTile = Strings.Hills;
                                    sourceNumber = Strings.TenImage;
                                }
                            }
                            else if (i == 3)
                            {
                                if (k == 1)
                                {
                                    sourceTile = Strings.FieldsOne;
                                    sourceNumber = Strings.NineImage;
                                }
                                else if (k == 2)
                                {
                                    sourceTile = Strings.ForestTwo;
                                    sourceNumber = Strings.ElevenImage;
                                }
                                else if (k == 3)
                                {
                                    sourceTile = Strings.Desert;
                                    sourceNumber = String.Empty;
                                }
                                else if (k == 4)
                                {
                                    sourceTile = Strings.ForestOne;
                                    sourceNumber = Strings.ThreeImage;
                                }
                                else
                                {
                                    sourceTile = Strings.MountienTwo;
                                    sourceNumber = Strings.EightImage;
                                }
                            }
                            else if (i == 4)
                            {
                                if (k == 1)
                                {
                                    sourceTile = Strings.ForestTwo;
                                    sourceNumber = Strings.EightImage;
                                }
                                else if (k == 2)
                                {
                                    sourceTile = Strings.MountienOne;
                                    sourceNumber = Strings.ThreeImage;
                                }
                                else if (k == 3)
                                {
                                    sourceTile = Strings.FieldsTwo;
                                    sourceNumber = Strings.FourImage;
                                }
                                else
                                {
                                    sourceTile = Strings.PastureOne;
                                    sourceNumber = Strings.FiveImage;
                                }
                            }
                            else
                            {
                                if (k == 1)
                                {
                                    sourceTile = Strings.Hills;
                                    sourceNumber = Strings.FiveImage;
                                }
                                else if (k == 2)
                                {
                                    sourceTile = Strings.FieldsTwo;
                                    sourceNumber = Strings.SixImage;
                                }
                                else
                                {
                                    sourceTile = Strings.PastureTwo;
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
                        game.TileTypes[(i - 1) * 5 + k - 1] = sourceTile;
                        Row.Add(CreateTileImage(sourceTile), k);
                        game.TileNumbers[(i - 1) * 5 + k - 1] = sourceNumber;
                        Row.Add(CreateNumberImage(sourceNumber), k);
                    }
                    gameBoard.Add(Row, 0, i);
                    Row = new()
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                }
                Dictionary<string, object> dict = new()
                {
                    {nameof(game.TileNumbers), game.TileNumbers },
                    {nameof(game.TileTypes), game.TileTypes }
                };
                game.UpdateFields(dict);
            }
            grdPices.RowDefinitions.Add(new RowDefinition { Height = new(8.4, GridUnitType.Star) });
            for (int i = 0; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    grdPices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                    grdPices.RowDefinitions.Add(new RowDefinition { Height = new(1.1, GridUnitType.Star) });
                    grdPices.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                }
                else
                {
                    grdPices.RowDefinitions.Add(new RowDefinition { Height = new(3.4, GridUnitType.Star) });
                }

            }
            grdPices.RowDefinitions.Add(new RowDefinition { Height = new(8.4, GridUnitType.Star) });
            for (int i = 1; i < 24; i++)
            {
                Row = new()
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                if (i % 2 != 0)
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < (i == 1 || i == 23 ? 4 : i == 3 || i == 5 || i == 19 || i == 21 ? 5 : i == 7 || i == 9 || i == 15 || i == 17 ? 6 : 7); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        BoardPiceButtons[i, k] = CreateApexButton(k, i);
                        BoardPiceButtons[i, k].Clicked += OnButtonClicked;
                        Row.Add(BoardPiceButtons[i, k], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    Row.ColumnSpacing = i == 11 || i == 13 ? 62 : 52;
                }
                else
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < (i == 2 || i == 22 ? 7 : i == 4 || i == 20 ? 5 : i == 6 || i == 18 ? 9 : i == 8 || i == 16 ? 6 : i == 10 || i == 14 ? 11 : 7); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        BoardPiceButtons[i, k] = CreateRoadButton(i % 4 == 0 ? 90 : k % 2 == 0 ? 30 : -30, k, i);
                        BoardPiceButtons[i, k].Clicked += OnButtonClicked;
                        Row.Add(BoardPiceButtons[i, k], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    Row.ColumnSpacing = i % 4 != 0 ? 12.6 : i == 12 ? 62.3 : 44;
                    Row.Rotation = i > 12 ? 180 : 0;
                }
                grdPices.Add(Row, 0, i);
            }
        }
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            ButtonClicked?.Invoke(this, (IndexedButton)sender!);
        }
        public override void ShowBuildOptions(string peiceType, Game game)
        {
            //    string[,] BoardPeices = new string[24, 12];
            //    for (int i = 1; i < 24; i++)
            //    {
            //        for (int k = 0; k < 12; k++)
            //        {
            //            if (game.BoardPeices[(i * 12) + k] != null)
            //                BoardPeices[i, k] = game.BoardPeices[(i * 12) + k];
            //        }
            //    }
            //    if (game.Status.CurrentStatus == GameStatus.Status.YourTurn)
            //    {
            //        if (peiceType == Strings.Road || peiceType == Strings.All)
            //        {
            //            for (int i = 1; i < 24; i = i + 2)
            //            {
            //                for (int k = 0; k < 12; k++)
            //                {
            //                    if (BoardPeices == (ToString(game.PlayerIndicator + 1) + Strings.City) || BoardPeices == (ToString(game.PlayerIndicator + 1) + Strings.Town))
            //                    {
            //                        if (i == 1)
            //                        {
            //                            if (game.BoardPeices[(k * 2) + 12] == 0)
            //                            {
            //                                BoardPiceButtons[2, (k * 2) + 1].BorderWidth = 1;
            //                            }
            //                            if (game.BoardPeices[(k * 2) + 13] == 0)
            //                            {
            //                                BoardPiceButtons[2, (k * 2) + 2].BorderWidth = 1;
            //                            }
            //                        }
            //                        else if (i == 3 || i == 7 || i == 11 || i == 15 || i == 19)
            //                        {
            //                            if (game.BoardPeices[((k % 12) * 2) + ((i - 2) * 12)] == 0)
            //                                BoardPiceButtons[i - 1, ((k % 12) * 2) + 1].BorderWidth = 1;

            //                            if (k % 12 != 0 && game.BoardPeices[((k % 12) * 2) + ((i - 2) * 12) - 1] == 0)
            //                                BoardPiceButtons[i - 1, ((k % 12) * 2)].BorderWidth = 1;

            //                            if (game.BoardPeices[k + 12] == 0)
            //                                BoardPiceButtons[i + 1, (k % 12) + 1].BorderWidth = 1;
            //                        }
            //                        else if (i == 5 || i == 9 || i == 13 || i == 17 || i == 21)
            //                        {
            //                            if (game.BoardPeices[((k % 12) * 2) + (i * 12)] == 0)
            //                            {
            //                                BoardPiceButtons[i + 1, ((k % 12) * 2) + 1].BorderWidth = 1;
            //                            }
            //                            if (game.BoardPeices[((k % 12) * 2) + (i * 12) + 1] == 0)
            //                            {
            //                                BoardPiceButtons[i + 1, ((k % 12) * 2) + 2].BorderWidth = 1;
            //                            }
            //                            if (game.BoardPeices[k - 12] == 0)
            //                            {
            //                                BoardPiceButtons[i - 1, (k % 12) + 1].BorderWidth = 1;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
        }
    }
}
