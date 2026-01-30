using CatanGame.Models;
using System;

namespace CatanGame.ModelsLogic
{
    public class GameGrid : GameGridModel
    {
        public GameGrid(Game game)
        {
            this.game = game;
            for (int i = 1; i < 24; i++)
            {
                BoardPiceButtons[i] = new IndexedButton[GetAmountOfColumns(i) - 1];
                BoardPiceImages[i] = new IndexedImage[GetAmountOfColumns(i) - 1];
            }
        }

        private static int GetBigestNumber(int num1, int num2, int num3 = 0, int num4 = 0)
        {
            return Math.Max(num1, Math.Max(num2, Math.Max(num3, num4)));
        }
        private static bool[][] IntBoolArray()
        {
            bool[][] array = new bool[12][];
            for (int i = 1; i < 12; i++)
                array[i] = new bool[GetAmountOfColumns(i * 2) - 1];
            return array;
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
                //Should not happen
                _ => 0,
            };
        }
        private static void GetFixedTile(int i, int k, out string sourceTile, out string sourceNumber)
        {
            // Determine the tile type and number based on fixed board layout
            (sourceTile, sourceNumber) = (i, k) switch
            {
                (1, 1) => (Strings.MountienOne, Strings.TenImage),
                (1, 2) => (Strings.PastureOne, Strings.TwoImage),
                (1, 3) => (Strings.ForestOne, Strings.NineImage),

                (2, 1) => (Strings.FieldsOne, Strings.TwelveImage),
                (2, 2) => (Strings.Hills, Strings.SixImage),
                (2, 3) => (Strings.PastureTwo, Strings.FourImage),
                (2, 4) => (Strings.Hills, Strings.TenImage),

                (3, 1) => (Strings.FieldsOne, Strings.NineImage),
                (3, 2) => (Strings.ForestTwo, Strings.ElevenImage),
                (3, 3) => (Strings.Desert, String.Empty),
                (3, 4) => (Strings.ForestOne, Strings.ThreeImage),
                (3, 5) => (Strings.MountienTwo, Strings.EightImage),

                (4, 1) => (Strings.ForestTwo, Strings.EightImage),
                (4, 2) => (Strings.MountienOne, Strings.ThreeImage),
                (4, 3) => (Strings.FieldsTwo, Strings.FourImage),
                (4, 4) => (Strings.PastureOne, Strings.FiveImage),

                (5, 1) => (Strings.Hills, Strings.FiveImage),
                (5, 2) => (Strings.FieldsTwo, Strings.SixImage),
                (5, 3) => (Strings.PastureTwo, Strings.ElevenImage),
                //Should not happen
                _ => (string.Empty, string.Empty),
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
                //Should not happen
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
        public static int GetTileLocationInArray(int row, int column)
        {
            int location = 0;
            for (int i = 1; i < row; i++)
            {
                location += GetAmountOfColumnsTiles(i);
            }
            for (int i = 1; i < column; i++)
            {
                location++;
            }
            return location;
        }
        public static int GetAmountOfColumnsTiles(int i)
        {
            return i switch
            {
                1 or 5 => 3,
                2 or 4 => 4,
                3 => 5,
                //Should not happen
                _ => 0,
            };
        }

        protected override void HideButtuns()
        {
            for (int i = 1; i < 24; i++)
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if(BoardPiceButtons[i][k].BorderWidth == Keys.ButtonVisible)
                        BoardPiceButtons[i][k].BorderWidth = 0;
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
                    CheckLongestRoad();
                }
                else if (button.RowIndex % 2 == 1 && ImageSource.IsNullOrEmpty(BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                {
                    BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPicesColor(game.PlayerIndicator + 1) + Strings.Town).ToLower();
                    button.BorderWidth = 0;
                    game.BoardPeices[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPiceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    HideButtuns();
                    if (game.Turn <= game.PlayerCount * 2)
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
        protected override void CheckLongestRoad()
        {
            for (int i = 1; i < 24; i++)
                for(int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (BoardPiceImages[i][k].Source != null && BoardPiceImages[i][k].Source.ToString()![6..] == GetPicesColor(game.PlayerIndicator + 1) + Strings.Road.ToLower())
                    {
                        bool[][] visited = IntBoolArray();
                        int roadLength = CheckLongestRoad(i, k,visited);
                        if (roadLength > game.PlayerLongestRoadLength)
                            game.PlayerLongestRoadLength = roadLength;
                    }
            if(game.PlayerLongestRoadLength > game.LongestRoadLength)
            {
                LongestRoad.Opacity = 1;
                game.LongestRoadLength  = game.PlayerLongestRoadLength;
                Dictionary<string, object> dict = new()
                {
                    { nameof(game.LongestRoadLength), game.LongestRoadLength }
                };
                game.UpdateFields(dict);
            }
        }
        protected override int CheckLongestRoad(int row, int column,bool[][] visited)
        {
            if (visited[row / 2][column])
                return 0;
            if (row == 2 && column != 0 && column != GetAmountOfColumns(row) - 2 && ((column % 2 == 0 && visited[(row / 2) + 1][column / 2] && visited[row / 2][column - 1]) || (column % 2 == 1 && visited[(row / 2) + 1][(column / 2) + 1] && visited[row / 2][column + 1])))
                return 0;
            if (row == 23 && column != 0 && column != GetAmountOfColumns(row) - 2 && ((column % 2 == 0 && visited[(row / 2) - 1][column / 2] && visited[row / 2][column - 1]) || (column % 2 == 1 && visited[(row / 2) - 1][(column / 2) + 1] && visited[row / 2][column + 1])))
                return 0;
            if (row < 12 && row % 4 == 0 && (column == 0 || column == GetAmountOfColumns(row) - 2) && (visited[(row / 2) + 1][column * 2] && visited[(row / 2) + 1][(column * 2) + 1]))
                return 0;
            if (row > 12 && row % 4 == 0 && (column == 0 || column == GetAmountOfColumns(row) - 2) && (visited[(row / 2) - 1][column * 2] && visited[(row / 2) - 1][(column * 2) + 1]))
                return 0;
            if (row % 4 == 0 && column != 0 && column != GetAmountOfColumns(row) - 2 && ((visited[(row / 2) + 1][column * 2] && visited[(row / 2) + 1][(column * 2) - 1]) || (visited[(row / 2) - 1][column * 2] && visited[(row / 2) - 1][(column * 2) - 1])))
                return 0;
            if ((row == 6 || row == 10) && ((column == 0 && visited[(row / 2) - 1][column] && visited[row / 2][column + 1]) || (column == GetAmountOfColumns(row) - 2 && visited[(row / 2) - 1][column / 2] && visited[row / 2][column - 1])))
                return 0;
            if ((row == 6 || row == 10 || row == 14 || row == 18) && (column != 0 && column != GetAmountOfColumns(row) - 2) && ((column % 2 == 0 && ((visited[(row / 2) - 1][column / 2] && visited[row / 2][column + 1]) || (visited[(row / 2) - 1][column / 2] && visited[row / 2][column - 1]))) || (column % 2 == 1 && ((visited[(row / 2) - 1][column / 2] && visited[row / 2][column - 1]) || (visited[(row / 2) + 1][(column / 2) + 1] && visited[row / 2][column + 1])))))
                return 0;
            if ((row == 14 || row == 18) && ((column == 0 && visited[(row / 2) + 1][column] && visited[row / 2][column + 1]) || (column == GetAmountOfColumns(row) - 2 && visited[(row / 2) + 1][column / 2] && visited[row / 2][column - 1])))
                return 0;
            visited[row / 2][column] = true;
            if (row < 12)
            {
                if (BoardPiceImages[row][column].Source.ToString()![6..] != GetPicesColor(game.PlayerIndicator + 1) + Strings.Road.ToLower())
                    return 0;
            }
            else
            {
                if (BoardPiceImages[row][GetAmountOfColumns(row) - 2 - column].Source.ToString()![6..] != GetPicesColor(game.PlayerIndicator + 1) + Strings.Road.ToLower())
                    return 0;
            }
            if (row == 2)
            {
                if (column == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row, column + 1, visited), CheckLongestRoad(row + 2, column, visited));
                if (column == GetAmountOfColumns(row) - 2)
                    return 1 + GetBigestNumber(CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row + 2, (column / 2) + 1, visited));
                if (column % 2 == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row, column + 1, visited), CheckLongestRoad(row + 2, column / 2, visited), CheckLongestRoad(row, column - 1, visited));
                return 1 + GetBigestNumber(CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row + 2, (column / 2) + 1, visited), CheckLongestRoad(row, column + 1, visited));
            }
            if (row == 23)
            {
                if (column == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row, column + 1, visited), CheckLongestRoad(row - 2, column, visited));
                if (column == GetAmountOfColumns(row) - 2)
                    return 1 + GetBigestNumber(CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row - 2, (column / 2) + 1, visited));
                if (column % 2 == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row, column + 1, visited), CheckLongestRoad(row - 2, column / 2, visited), CheckLongestRoad(row, column - 1, visited));
                return 1 + GetBigestNumber(CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row - 2, (column / 2) + 1, visited), CheckLongestRoad(row, column + 1, visited));
            }
            if (row % 4 == 0)
            {
                if (row < 12 && column == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, column, visited), CheckLongestRoad(row + 2, column + 1, visited), CheckLongestRoad(row - 2, column, visited));
                if (row < 12 && column == GetAmountOfColumns(row) - 2)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, GetAmountOfColumns(row + 2) - 2, visited), CheckLongestRoad(row + 2, GetAmountOfColumns(row + 2) - 3, visited), CheckLongestRoad(row - 2, GetAmountOfColumns(row - 2) - 2, visited));
                if (row == 12 && column == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, column, visited), CheckLongestRoad(row - 2, column, visited));
                if (row == 12 && column == GetAmountOfColumns(row) - 2)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, GetAmountOfColumns(row + 2) - 2, visited), CheckLongestRoad(row - 2, GetAmountOfColumns(row - 2) - 2, visited));
                if (row > 12 && column == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row - 2, column, visited), CheckLongestRoad(row - 2, column + 1, visited), CheckLongestRoad(row + 2, column, visited));
                if (row > 12 && column == GetAmountOfColumns(row) - 2)
                    return 1 + GetBigestNumber(CheckLongestRoad(row - 2, GetAmountOfColumns(row - 2) - 2, visited), CheckLongestRoad(row - 2, GetAmountOfColumns(row - 2) - 3, visited), CheckLongestRoad(row + 2, GetAmountOfColumns(row + 2) - 2, visited));
                return 1 + GetBigestNumber(CheckLongestRoad(row + 2, (column * 2) - 1, visited), CheckLongestRoad(row + 2, (column * 2), visited), CheckLongestRoad(row - 2, (column * 2) - 1, visited), CheckLongestRoad(row - 2, (column * 2), visited));
            }
            else
            {
                if (column == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, column, visited), CheckLongestRoad(row - 2, column, visited), CheckLongestRoad(row, column + 1, visited));
                if (column == GetAmountOfColumns(row) - 2)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, GetAmountOfColumns(row + 2) - 2, visited), CheckLongestRoad(row - 2, GetAmountOfColumns(row - 2) - 2, visited), CheckLongestRoad(row, column - 1, visited));
                if (column % 2 == 0)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, column / 2, visited), CheckLongestRoad(row, column + 1, visited), CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row - 2, column / 2, visited));
                if (row < 12)
                    return 1 + GetBigestNumber(CheckLongestRoad(row + 2, column / 2 + 1, visited), CheckLongestRoad(row - 2, column / 2, visited), CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row, column + 1, visited));
                return 1 + GetBigestNumber(CheckLongestRoad(row - 2, column / 2 + 1, visited), CheckLongestRoad(row + 2, column / 2, visited), CheckLongestRoad(row, column - 1, visited), CheckLongestRoad(row, column + 1, visited));
            }
        }

        public override void OnChange()
        {
            if (game.BoardPeices != null && BoardPiceImages != null && BoardPiceButtons != null)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPiceImages[i][k].Source != null && game.BoardPeices[((i - 1) * 12) + k] != null && BoardPiceImages[i][k].Source.ToString()![..6] != game.BoardPeices[((i - 1) * 12) + k])
                            BoardPiceImages[i][k].Source = game.BoardPeices[((i - 1) * 12) + k];
            if(LongestRoad.Opacity != Keys.DoesNotOwn && game.PlayerLongestRoadLength < game.LongestRoadLength)
                LongestRoad.Opacity = Keys.DoesNotOwn;
            if (LargestArmy.Opacity != Keys.DoesNotOwn && game.PlayerLargestArmySize < game.LargestArmySize)
                LargestArmy.Opacity = Keys.DoesNotOwn;

        }
        public override void Init(Grid gameBoard, Grid grdPices,Grid otherPices)
        {
            // Define the Rows In gameBoard (for UI/UX layout purposes)
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
            //Initialize the tiles on the UI/UX game board
            if (game.PlayerIndicator != 0)
                for (int i = 1; i < 6; i++)
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < 1 + GetAmountOfColumnsTiles(i); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(2, GridUnitType.Star) });
                        Row.Add(CreateTileImage(game.TileTypes[GetTileLocationInArray(i, k)]), k);
                        Row.Add(CreateNumberImage(game.TileNumbers[GetTileLocationInArray(i, k)]), k);
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
                //local variables Used for random board generation
                string sourceTile;
                string sourceNumber;
                Random random = new();
                int count = 0;
                //string arrays containing all the tile types and numbers, bye there respective amount
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
                        // Determine the tile type and number based on whether the board is random or fixed
                        if (game.IsRandomBoard)
                        {

                            int curent = random.Next(0, tiles.Length - count);
                            sourceTile = tiles[curent];
                            tiles[curent] = String.Empty;
                            //Desert tile does not get a number token
                            if (sourceTile == Strings.Desert)
                                sourceNumber = String.Empty;
                            else
                            {
                                curent = random.Next(0, numbers.Length - count - 1);
                                sourceNumber = numbers[curent];
                                numbers[curent] = String.Empty;
                            }
                            count++;
                            //Shift the arrays to remove empty entries
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
                        else
                        {
                            GetFixedTile(i, k, out sourceTile, out sourceNumber);
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
                //Update the firebase with the new tile types and numbers
                game.UpdateFields(dict);
            }
            // Define the Rows In grdPices (for UI/UX layout purposes)
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
            //Initialize the peices on the game UI/UX board
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
            //Connect the game logic board with the UI/UX board
            game.GameBoard.InitBoard(BoardPiceButtons, game.TileTypes, game.TileNumbers);
            if (game.PlayerIndicator == 0)
                ShowBuildOptions(Strings.Town);
            otherPices.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            otherPices.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            LongestRoad = new()
            {
                Source = Strings.LongestRoadImage,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                Opacity = Keys.DoesNotOwn
            };
            otherPices.Add(LongestRoad);
            LargestArmy = new()
            {
                Source = Strings.LargestArmyImage,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                Opacity = Keys.DoesNotOwn
            };
            otherPices.Add(LargestArmy, 1, 0);
            otherPices.ColumnSpacing = 5;
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
                                    if(i == 13)
                                    {
                                        if (BoardPeices[i - 1][k] == string.Empty)
                                            BoardPiceButtons[i - 1][k].BorderWidth = Keys.ButtonVisible;
                                    }
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
