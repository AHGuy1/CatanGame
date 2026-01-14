using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class GameGrid : GameGridModel
    {
        private readonly Game game;
        public GameGrid(Game game)
        {
            this.game = game;
            for (int i = 1; i < 24; i++)
            {
                BoardPiceButtons[i] = new IndexedButton[GetAmountOfColumns(i) - 1];
                BoardPiceImages[i] = new IndexedImage[GetAmountOfColumns(i) - 1];
            }
        }

        private static int GetAmountOfColumns(int i)
        {
            return i switch
            {
                1 or 23 => 4,
                3 or 4 or 5 or 19 or 20 or 21 => 5,
                7 or 8 or 9 or 15 or 16 or 17 => 6,
                2 or 11 or 12 or 13 or 22 => 7,
                6 or 18 => 9,
                10 or 14 => 11,
                _ => 0,
            };
        }
        private static string GetPicesColor(int i)
        {
            return i switch
            {
                1 => Strings.Oreange,
                2 => Strings.Blue,
                3 => Strings.Yellow,
                4 => Strings.Red,
                5 => Strings.Green,
                6 => Strings.Cyan,
                _ => string.Empty,
            };
        }
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
        private static IndexedButton CreateRoadButton(int rotation, int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, 6*1.1, 18*1.1, rotation);
        }
        private static IndexedButton CreateApexButton(int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, 10 * 1.5, 10 * 1.4);
        }
        private static IndexedImage CreateRoadImage(int rotation, int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, 6 * 1.1, 18 * 1.1, rotation);
        }
        private static IndexedImage CreateApexImage(int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, 10 * 1.5, 10 * 1.5);
        }

        protected override void HideButtuns()
        {
            for (int i = 1; i < 24; i++)
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    BoardPiceButtons[i][k].BorderWidth = 0;
        }

        public override void Init(Grid gameBoard, Grid grdPices)
        {
            gameBoard.RowDefinitions.Add(new RowDefinition { Height = new(0, GridUnitType.Star) });
            for (int i = 0; i < 5; i++)
                gameBoard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            gameBoard.RowDefinitions.Add(new RowDefinition { Height = new(1.75, GridUnitType.Star) });
            gameBoard.RowSpacing = 0;
            Grid Row = new()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            if (game.PlayerIndicator != 0)
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
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(2, GridUnitType.Star) });
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
                    grdPices.RowDefinitions.Add(new RowDefinition { Height = new(3.4, GridUnitType.Star) });
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
                    for (int k = 1; k < GetAmountOfColumns(i); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        BoardPiceButtons[i][k - 1] = CreateApexButton(k, i);
                        BoardPiceButtons[i][k - 1].Clicked += OnButtonClicked;
                        Row.Add(BoardPiceButtons[i][k - 1], k);
                        BoardPiceImages[i][k - 1] = CreateApexImage(k, i);
                        BoardPiceImages[i][k - 1].Source = game.BoardPeices[(i-1)*12 + k -1];
                        Row.Add(BoardPiceImages[i][k - 1], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    Row.ColumnSpacing = i == 11 || i == 13 ? 63 : 48;
                }
                else
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
                    for (int k = 1; k < GetAmountOfColumns(i); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        BoardPiceButtons[i][k - 1] = CreateRoadButton(i % 4 == 0 ? 90 : k % 2 == 0 ? 30 : -30, k, i);
                        BoardPiceButtons[i][k - 1].Clicked += OnButtonClicked;
                        Row.Add(BoardPiceButtons[i][k - 1], k);
                        BoardPiceImages[i][k - 1] = CreateRoadImage(i % 4 == 0 ? 90 : k % 2 == 0 ? 30 : -30, k, i);
                        BoardPiceImages[i][k - 1].Source = game.BoardPeices[(i - 1) * 12 + k - 1];
                        Row.Add(BoardPiceImages[i][k - 1], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
                    Row.ColumnSpacing = i % 4 == 0 ? i == 12 ? 62.3 : 43 : 12;
                    Row.Rotation = i > 12 ? 180 : 0;
                }
                grdPices.Add(Row, 0, i);
            }
            if (game.PlayerIndicator == 0)
                ShowBuildOptions(Strings.All);
        }
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            IndexedButton? button = (IndexedButton)sender!;
            if (button.BorderWidth == Keys.ButtonVisible)
            {
                if (button.RowIndex % 2 == 0 && ImageSource.IsNullOrEmpty(BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                {
                    BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPicesColor(game.PlayerIndicator + 1) + Strings.Road).ToLower();
                    button.BorderWidth = 0;
                    game.BoardPeices[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    HideButtuns();
                }
                else if (button.RowIndex % 2 == 1 && ImageSource.IsNullOrEmpty(BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                {
                    BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPicesColor(game.PlayerIndicator + 1) + Strings.Town).ToLower();
                    button.BorderWidth = 0;
                    game.BoardPeices[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    HideButtuns();
                    if (game.Turn <= game.PlayerCount*2)
                        ShowBuildOptions(Strings.Road);
                }
                else if (button.RowIndex % 2 == 1 && BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()!.Contains(GetPicesColor(game.PlayerIndicator + 1) + Strings.Town.ToLower()))
                {
                    BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPicesColor(game.PlayerIndicator + 1) + Strings.City).ToLower();
                    button.BorderWidth = 0;
                    game.BoardPeices[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    HideButtuns();
                }
                Dictionary<string, object> dict = new()
                {
                    { nameof(game.BoardPeices), game.BoardPeices },
                };
                game.UpdateFields(dict);
            }
        }
        public override void OnChange()
        {
            if (game.BoardPeices != null && BoardPiceImages != null && BoardPiceButtons != null)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPiceImages[i][k].Source != null && game.BoardPeices[((i - 1) * 12) + k] != null && BoardPiceImages[i][k].Source.ToString() != game.BoardPeices[((i - 1) * 12) + k])
                            BoardPiceImages[i][k].Source = game.BoardPeices[((i - 1) * 12) + k];
        }

        public override void ShowBuildOptions(string peiceType)
        {
            string[][] BoardPeices = new string[24][];
            for (int i = 1; i < 24; i++)
            {
                BoardPeices[i] = new string[GetAmountOfColumns(i) - 1];
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (game.BoardPeices[((i - 1) * 12) + k] != null)
                        BoardPeices[i][k] = game.BoardPeices[((i - 1) * 12) + k];
            }
            if (peiceType == Strings.Road || peiceType == Strings.All)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPeices[i][k].Equals(GetPicesColor(game.PlayerIndicator + 1) + Strings.City.ToLower()) || BoardPeices[i][k].Equals(GetPicesColor(game.PlayerIndicator + 1) + Strings.Town.ToLower()))
                        {
                            if (i == 1)
                            {
                                if (BoardPeices[i + 1][k * 2] == string.Empty)
                                    BoardPiceButtons[i + 1][k * 2].BorderWidth = Keys.ButtonVisible;
                                if (BoardPeices[i + 1][(k * 2) + 1] == string.Empty)
                                    BoardPiceButtons[i + 1][(k * 2) + 1].BorderWidth = Keys.ButtonVisible;
                            }
                            else if (i == 3 || i == 7 || i == 11 || i == 15 || i == 19)
                            {

                                if (i > 12)
                                {
                                    if (BoardPeices[i + 1][GetAmountOfColumns(i + 1) - 2 - k] == string.Empty)
                                        BoardPiceButtons[i + 1][GetAmountOfColumns(i + 1) - 2 - k].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i - 1][GetAmountOfColumns(i - 1) - (k * 2) - 3] == string.Empty)
                                        BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - (k * 2) - 3].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i - 1][GetAmountOfColumns(i - 1) - (k * 2) - 2] == string.Empty)
                                        BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - (k * 2) - 2].BorderWidth = Keys.ButtonVisible;
                                }
                                else
                                {
                                    if (BoardPeices[i + 1][k] == string.Empty)
                                        BoardPiceButtons[i + 1][k].BorderWidth = Keys.ButtonVisible;
                                    if (k != GetAmountOfColumns(i) - 2 && BoardPeices[i - 1][k * 2] == string.Empty)
                                        BoardPiceButtons[i - 1][k * 2].BorderWidth = Keys.ButtonVisible;
                                    if (k != 0 && BoardPeices[i - 1][(k * 2) - 1] == string.Empty)
                                        BoardPiceButtons[i - 1][(k * 2) - 1].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                            else if (i == 5 || i == 9 || i == 13 || i == 17 || i == 21)
                            {
                                if (i < 12)
                                {
                                    if (BoardPeices[i - 1][k] == string.Empty)
                                        BoardPiceButtons[i - 1][k].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][(k * 2) + 1] == string.Empty)
                                        BoardPiceButtons[i + 1][(k * 2) + 1].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][k * 2] == string.Empty)
                                        BoardPiceButtons[i + 1][k * 2].BorderWidth = Keys.ButtonVisible;
                                }
                                else
                                {
                                    if(i== 13)
                                        if (BoardPeices[i - 1][k] == string.Empty)
                                            BoardPiceButtons[i - 1][k].BorderWidth = Keys.ButtonVisible;
                                    else if (BoardPeices[i - 1][GetAmountOfColumns(i - 1) - 2 - k] == string.Empty)
                                        BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - 2 - k].BorderWidth = Keys.ButtonVisible;    
                                    if (k < GetAmountOfColumns(i) - 2 && BoardPeices[i + 1][GetAmountOfColumns(i + 1) - (k * 2) - 2] == string.Empty)
                                        BoardPiceButtons[i + 1][GetAmountOfColumns(i + 1) - (k * 2) - 2].BorderWidth = Keys.ButtonVisible;
                                    if (k > 0 && BoardPeices[i + 1][GetAmountOfColumns(i + 1) - (k * 2) - 1] == string.Empty)
                                        BoardPiceButtons[i + 1][GetAmountOfColumns(i + 1) - (k * 2) - 1].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                            else if (i == 23)
                            {
                                if (BoardPeices[i - 1][GetAmountOfColumns(i - 1) - (k * 2) - 2] == string.Empty)
                                    BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - (k * 2) - 2].BorderWidth = Keys.ButtonVisible;
                                if (BoardPeices[i - 1][GetAmountOfColumns(i - 1) - (k * 2)-3] == string.Empty)
                                    BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - (k * 2)-3].BorderWidth = Keys.ButtonVisible;
                            }
                        }
            if (peiceType == Strings.All)
            {         
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPeices[i][k].Equals(GetPicesColor(game.PlayerIndicator + 1) + Strings.Road.ToLower()))
                        {
                            if (i == 2 || i == 6 || i == 10)
                            {
                                if (k % 2 == 0)
                                {
                                    if (BoardPeices[i - 1][k / 2] == string.Empty)
                                        BoardPiceButtons[i - 1][k / 2].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][k / 2] == string.Empty)
                                        BoardPiceButtons[i + 1][k / 2].BorderWidth = Keys.ButtonVisible;
                                }
                                else
                                {
                                    if (BoardPeices[i - 1][k / 2] == string.Empty)
                                        BoardPiceButtons[i - 1][k / 2].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][k / 2 + 1] == string.Empty)
                                        BoardPiceButtons[i + 1][k / 2 + 1].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                            else if (i == 14 || i == 18 || i == 22)
                            {
                                if (k % 2 == 0)
                                {
                                    if (BoardPeices[i - 1][GetAmountOfColumns(i-1) -( k / 2)-2] == string.Empty)
                                        BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - (k / 2)-2].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][GetAmountOfColumns(i + 1) - (k / 2)-2] == string.Empty)
                                        BoardPiceButtons[i + 1][GetAmountOfColumns(i + 1) - (k / 2) - 2].BorderWidth = Keys.ButtonVisible;
                                }
                                else
                                {
                                    if (BoardPeices[i + 1][GetAmountOfColumns(i + 1) - (k / 2) - 2] == string.Empty)
                                        BoardPiceButtons[i + 1][GetAmountOfColumns(i + 1) - (k / 2) - 2].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i - 1][GetAmountOfColumns(i - 1) - (k / 2)-3] == string.Empty)
                                        BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - (k / 2)-3].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                            else if (i == 4 || i == 8 || i == 12 || i == 16 || i == 20)
                            {
                                if(i>12)
                                {
                                    if (BoardPeices[i - 1][GetAmountOfColumns(i-1)- k - 2] == string.Empty)
                                        BoardPiceButtons[i - 1][GetAmountOfColumns(i - 1) - k - 2].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][GetAmountOfColumns(i + 1) - k - 2] == string.Empty)
                                        BoardPiceButtons[i + 1][GetAmountOfColumns(i + 1) - k - 2].BorderWidth = Keys.ButtonVisible;
                                }
                                else
                                {
                                    if (BoardPeices[i - 1][k] == string.Empty)
                                        BoardPiceButtons[i - 1][k].BorderWidth = Keys.ButtonVisible;
                                    if (BoardPeices[i + 1][k] == string.Empty)
                                        BoardPiceButtons[i + 1][k].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                        }
            }
            if (peiceType == Strings.City || peiceType == Strings.All)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPeices[i][k].Equals(GetPicesColor(game.PlayerIndicator + 1) + Strings.Town.ToLower()))
                            BoardPiceButtons[i][k].BorderWidth = Keys.ButtonVisible;
            if (peiceType == Strings.Town)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPeices[i][k].Equals(string.Empty) && i % 2 == 1)
                            BoardPiceButtons[i][k].BorderWidth = Keys.ButtonVisible;

        }
    }
}
