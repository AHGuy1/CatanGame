using CatanGame.Models;
using CatanGame.Platforms.Android;
using SkiaSharp.Extended.UI.Controls;
using static Android.InputMethodServices.Keyboard;

namespace CatanGame.ModelsLogic
{
    public class GameGrid : GameGridModel
    {
        public GameGrid(Game game)
        {
            this.game = game;
            for (int i = 1; i < 24; i++)
            {
                BoardPieceButtons[i] = new IndexedButton[GetAmountOfColumns(i) - 1];
                BoardPieceImages[i] = new IndexedImage[GetAmountOfColumns(i) - 1];
            }
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
        private static string GetPiecesColor(int i)
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
        private static double GetSizeProportion()
        {
            Microsoft.Maui.Devices.DisplayInfo mainDisplay = Microsoft.Maui.Devices.DeviceDisplay.Current.MainDisplayInfo;
            if (mainDisplay.Height < mainDisplay.Width)
                return mainDisplay.Height / mainDisplay.Density;
            return mainDisplay.Width / mainDisplay.Density;
        }
        private static Grid CreateTileImage(string imageSource)
        {
            Grid grid = [];
            Image image = new()
            {
                Source = imageSource,
                HeightRequest = GetSizeProportion() * 0.185,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Button button = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = GetSizeProportion() * 0.12,
                WidthRequest = GetSizeProportion() * 0.12,
                BackgroundColor = Colors.Transparent,
                IsEnabled = false
            };
            grid.Add(button);
            grid.Add(image);
            return grid;
        }
        private static Grid CreateEmptyCenteredGrid()
        {
            return new()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
        }
        private static SKLottieView CreateDiceAnimation()
        {
            double sizeProportion = GetSizeProportion();
            return new()
            {
                Source = new SKFileLottieImageSource { File = Strings.DiceRollAnimation },
                RepeatCount = -1,
                IsVisible = false,
                WidthRequest = sizeProportion * 0.25,
                HeightRequest = sizeProportion * 0.25,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
        }
        private static Image CreateDiceImage()
        {
            double sizeProportion = GetSizeProportion();
            return new()
            {
                Source = Strings.DiceSixImage,
                WidthRequest = sizeProportion * 0.15,
                HeightRequest = sizeProportion * 0.15,
                IsVisible = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
        }
        private static Image CreateNumberImage(string imageSource)
        {
            return new()
            {
                Source = imageSource,
                HeightRequest = GetSizeProportion() * 0.06,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }
        private static IndexedButton CreateRoadButton(int rotation, int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, GetSizeProportion() * 0.0165, GetSizeProportion() * 0.0495, rotation);
        }
        private static IndexedButton CreateApexButton(int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, GetSizeProportion() * 0.03525, GetSizeProportion() * 0.03525);
        }
        private static IndexedImage CreateRoadImage(int rotation, int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, GetSizeProportion() * 0.0165, GetSizeProportion() * 0.0495, rotation);
        }
        private static IndexedImage CreateApexImage(int colmnIndex, int rowIndex)
        {
            return new(rowIndex, colmnIndex, GetSizeProportion() * 0.03525, GetSizeProportion() * 0.03525);
        }
        private static string GetDiceImage(int dice)
        {
            return dice == 1 ? Strings.DiceOneImage :
                dice == 2 ? Strings.DiceTwoImage :
                dice == 3 ? Strings.DiceThreeImage :
                dice == 4 ? Strings.DiceFourImage :
                dice == 5 ? Strings.DiceFiveImage :
                Strings.DiceSixImage;
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
        public static int GetAmountOfColumns(int i)
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
        public static int GetPieceLocationInArray(int row, int column)
        {
            int location = 0;
            if (row % 2 == 0)
            {
                for (int i = 2; i < row; i += 2)
                {
                    location += GetAmountOfColumns(i) - 1;
                }
                for (int i = 0; i < column; i++)
                {
                    location++;
                }
                return location;
            }
            else
            {
                for (int i = 1; i < row; i += 2)
                {
                    location += GetAmountOfColumns(i) - 1;
                }
                for (int i = 0; i < column; i++)
                {
                    location++;
                }
                return location;
            }
        }
        protected override BoardModel.PieceType GetPieceType(int row, int column)
        {
            return BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Town, StringComparison.CurrentCultureIgnoreCase) ? BoardModel.PieceType.Town :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.City, StringComparison.CurrentCultureIgnoreCase) ? BoardModel.PieceType.City :
                   BoardModel.PieceType.None;
        }
        protected override int GetPieceIndexFromColor(int row, int column)
        {
            return BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Oreange, StringComparison.CurrentCultureIgnoreCase) ? 0 :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Blue, StringComparison.CurrentCultureIgnoreCase) ? 1 :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Yellow, StringComparison.CurrentCultureIgnoreCase) ? 2 :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Red, StringComparison.CurrentCultureIgnoreCase) ? 3 :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Green, StringComparison.CurrentCultureIgnoreCase) ? 4 :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Cyan, StringComparison.CurrentCultureIgnoreCase) ? 5 :
                   -1;
        }
        protected override void HideButtuns()
        {
            for (int i = 1; i < 24; i++)
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (BoardPieceButtons[i][k].BorderWidth == Keys.ButtonVisible)
                        BoardPieceButtons[i][k].BorderWidth = 0;
        }
        protected override void OnBuildOptionsButtonClicked(object? sender, EventArgs e)
        {
            IndexedButton? button = (IndexedButton)sender!;
            if (button.BorderWidth == Keys.ButtonVisible)
            {
                if (button.RowIndex % 2 == 0 && ImageSource.IsNullOrEmpty(BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                {
                    BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPiecesColor(game.PlayerIndicator + 1) + Strings.Road).ToLower();
                    game.BoardPieces[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    game.GameBoard.Edges[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].RoadOwnerPlayerIndex = GetPieceIndexFromColor(button.RowIndex, button.ColumnIndex - 1);
                    CheckLongestRoad();
                }
                else
                {
                    if (button.RowIndex % 2 == 1 && ImageSource.IsNullOrEmpty(BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                    {
                        BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPiecesColor(game.PlayerIndicator + 1) + Strings.Town).ToLower();
                        game.BoardPieces[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    }
                    else if (button.RowIndex % 2 == 1 && BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()!.Contains(GetPiecesColor(game.PlayerIndicator + 1) + Strings.Town.ToLower()))
                    {
                        BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPiecesColor(game.PlayerIndicator + 1) + Strings.City).ToLower();
                        game.BoardPieces[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                    }
                    game.GameBoard.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PlayerIndex = GetPieceIndexFromColor(button.RowIndex, button.ColumnIndex - 1);
                    game.GameBoard.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PieceType = GetPieceType(button.RowIndex, button.ColumnIndex - 1);
                    Dictionary<string, object> dict = new()
                    {
                        { nameof(game.BoardPieces), game.BoardPieces }
                    };
                    game.UpdateFields(dict);
                }
                HideButtuns();
                if (game.Turn <= game.PlayerCount * 2 && button.RowIndex % 2 == 1 && game.GameBoard.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PlayerIndex == game.PlayerIndicator && game.GameBoard.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PieceType == BoardModel.PieceType.Town)
                    ShowBuildOptions(Strings.Road);
            }
        }
        protected override void CheckLongestRoad()
        {
            EdgeLink[] edges = game.GameBoard.Edges;
            for (int i = 0; i < edges.Length; i++)
            {
                if (edges[i].RoadOwnerPlayerIndex == game.PlayerIndicator)
                {
                    bool[] visited = new bool[edges.Length];
                    int roadLength = CheckLongestRoad(edges[i], visited);
                    if (roadLength > game.PlayerLongestRoadLength)
                        game.PlayerLongestRoadLength = roadLength;
                }
            }
            if (game.PlayerLongestRoadLength > game.LongestRoadLength)
            {
                LongestRoad.Opacity = 1;
                game.LongestRoadLength = game.PlayerLongestRoadLength;
            }
            Dictionary<string, object> dict = new()
                {
                    { nameof(game.BoardPieces), game.BoardPieces },
                    { nameof(game.LongestRoadLength), game.LongestRoadLength }
                };
            game.UpdateFields(dict);
        }
        protected override int CheckLongestRoad(EdgeLink edge, bool[] visited)
        {
            if (visited[GetPieceLocationInArray(edge.Row, edge.Column)])
                return 0;
            int longestBranch = 0;
            int curentBranch;
            visited[GetPieceLocationInArray(edge.Row, edge.Column)] = true;
            //if vertex not owned by another player
            if (edge.VertexNodeOne.PlayerIndex == -1 || edge.VertexNodeOne.PlayerIndex == game.PlayerIndicator)
            {
                EdgeLink[] edges = edge.VertexNodeOne.Edges;
                if (edges.Length > 2)
                {
                    //Handele Forks
                    bool a = visited[GetPieceLocationInArray(edges[0].Row, edges[0].Column)];
                    bool b = visited[GetPieceLocationInArray(edges[1].Row, edges[1].Column)];
                    bool c = visited[GetPieceLocationInArray(edges[2].Row, edges[2].Column)];
                    if (!(a && b) && !(b && c) && !(c && a))
                        for (int i = 0; i < 3; i++)
                            if (edges[i].RoadOwnerPlayerIndex == game.PlayerIndicator)
                            {
                                curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                                if (curentBranch > longestBranch)
                                    longestBranch = curentBranch;
                            }
                }
                else
                    for (int i = 0; i < 2; i++)
                        if (edges[i].RoadOwnerPlayerIndex == game.PlayerIndicator)
                        {
                            curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                            if (curentBranch > longestBranch)
                                longestBranch = curentBranch;
                        }
            }
            if (edge.VertexNodeTwo.PlayerIndex == -1 || edge.VertexNodeTwo.PlayerIndex == game.PlayerIndicator)
            {
                EdgeLink[] edges = edge.VertexNodeTwo.Edges;
                if (edges.Length > 2)
                {
                    //Handele Forks
                    bool a = visited[GetPieceLocationInArray(edges[0].Row, edges[0].Column)];
                    bool b = visited[GetPieceLocationInArray(edges[1].Row, edges[1].Column)];
                    bool c = visited[GetPieceLocationInArray(edges[2].Row, edges[2].Column)];
                    if (!(a && b) && !(b && c) && !(c && a))
                        for (int i = 0; i < 3; i++)
                            if (edges[i].RoadOwnerPlayerIndex == game.PlayerIndicator)
                            {
                                curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                                if (curentBranch > longestBranch)
                                    longestBranch = curentBranch;
                            }
                }
                else
                    for (int i = 0; i < 2; i++)
                        if (edges[i].RoadOwnerPlayerIndex == game.PlayerIndicator)
                        {
                            curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                            if (curentBranch > longestBranch)
                                longestBranch = curentBranch;
                        }
            }
            visited[GetPieceLocationInArray(edge.Row, edge.Column)] = false;
            return longestBranch;
        }
        protected override void OnRollButtonClicked(object? sender, EventArgs e)
        {
            RollButton.IsEnabled = false;
            Random random = new();
            game.Roll1 = random.Next(1, 7);
            game.Roll2 = random.Next(1, 7);
            game.IsRolling = true;
            StartAnimations();
            Dictionary<string, object> dict = new()
            {
                { nameof(game.Roll1), game.Roll1 },
                { nameof(game.Roll2), game.Roll2 },
                { nameof(game.IsRolling), game.IsRolling }
            };
            game.UpdateFields(OnDiceUpdated, dict);
        }
        protected override async void OnDiceUpdated(Task task)
        {
            await Task.Delay(2000);
            if (task.IsCompletedSuccessfully)
            {
                StopAnimations();
                game.IsRolling = false;
                Dictionary<string, object> dict = new()
                {
                    { nameof(game.IsRolling), game.IsRolling }
                };
                game.UpdateFields(dict);
            }
        }
        protected override void StartAnimations()
        {
            Dice1Image.IsVisible = false;
            Dice2Image.IsVisible = false;
            Dice1Roll.Progress = TimeSpan.Zero;
            Dice2Roll.Progress = TimeSpan.Zero;
            Dice1Roll.IsVisible = true;
            Dice2Roll.IsVisible = true;
            Dice1Roll.IsAnimationEnabled = true;
            Dice2Roll.IsAnimationEnabled = true;
            RollLabel.Text = Strings.Rolling;
        }
        protected override void StopAnimations()
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Dice1Roll.IsVisible = false;
                Dice1Roll.IsAnimationEnabled = false;
                Dice1Image.Source = GetDiceImage(game.Roll1);
                Dice1Image.IsVisible = true;
                Dice2Roll.IsAnimationEnabled = false;
                Dice2Image.Source = GetDiceImage(game.Roll2);
                Dice2Image.IsVisible = true;
                Dice2Roll.IsVisible = false;
                RollLabel.Text = Strings.Rolled + game.RollTotal;
            });
        }

        public override void OnAnimationStatusChanged()
        {
            if (game.IsRolling)
                StartAnimations();
            else
                StopAnimations();
        }
        public override void OnChange()
        {
            if (game.BoardPieces != null && BoardPieceImages != null && BoardPieceButtons != null)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPieceImages[i][k].Source != null && game.BoardPieces[((i - 1) * 12) + k] != null && BoardPieceImages[i][k].Source.ToString()![6..] != game.BoardPieces[((i - 1) * 12) + k])
                        {
                            BoardPieceImages[i][k].Source = game.BoardPieces[((i - 1) * 12) + k];
                            if (i % 2 == 0)
                                game.GameBoard.Edges[GetPieceLocationInArray(i, k)].RoadOwnerPlayerIndex = GetPieceIndexFromColor(i, k);
                            else
                            {
                                game.GameBoard.Vertices[GetPieceLocationInArray(i, k)].PlayerIndex = GetPieceIndexFromColor(i, k);
                                game.GameBoard.Vertices[GetPieceLocationInArray(i, k)].PieceType = GetPieceType(i, k);
                            }
                        }
            if (LongestRoad.Opacity != Keys.DoesNotOwn && game.PlayerLongestRoadLength < game.LongestRoadLength)
                LongestRoad.Opacity = Keys.DoesNotOwn;
            if (LargestArmy.Opacity != Keys.DoesNotOwn && game.PlayerLargestArmySize < game.LargestArmySize)
                LargestArmy.Opacity = Keys.DoesNotOwn;

        }
        public override void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame)
        {
            double sizeProportion = GetSizeProportion();
            frame.WidthRequest = sizeProportion * 1.095;
            frame.HeightRequest = sizeProportion * 0.95;
            double gridSize = sizeProportion * 0.975;
            gameBoard.WidthRequest = gridSize;
            gameBoard.HeightRequest = gridSize;
            grdPieces.WidthRequest = gridSize;
            grdPieces.HeightRequest = gridSize;
            // Define the Rows In gameBoard (for UI/UX layout purposes)
            gameBoard.RowDefinitions.Add(new RowDefinition { Height = new(0.95, GridUnitType.Star) });
            for (int i = 0; i < 5; i++)
                gameBoard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            gameBoard.RowDefinitions.Add(new RowDefinition { Height = new(1, GridUnitType.Star) });
            gameBoard.RowSpacing = 0;
            Grid Row = CreateEmptyCenteredGrid();
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
                    Row = CreateEmptyCenteredGrid();
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
                        game.TileTypes[GetTileLocationInArray(i, k)] = sourceTile;
                        Row.Add(CreateTileImage(sourceTile), k);
                        game.TileNumbers[GetTileLocationInArray(i, k)] = sourceNumber;
                        Row.Add(CreateNumberImage(sourceNumber), k);
                    }
                    gameBoard.Add(Row, 0, i);
                    Row = CreateEmptyCenteredGrid();
                }
                Dictionary<string, object> dict = new()
                {
                    {nameof(game.TileNumbers), game.TileNumbers },
                    {nameof(game.TileTypes), game.TileTypes }
                };
                //Update the firebase with the new tile types and numbers
                game.UpdateFields(dict);
            }
            //Connect the game logic board with the UI/UX board
            game.GameBoard.InitBoard(BoardPieceButtons, game.TileTypes, game.TileNumbers);
            // Define the Rows In grdPieces (for UI/UX layout purposes)
            grdPieces.RowDefinitions.Add(new RowDefinition { Height = new(5, GridUnitType.Star) });
            for (int i = 0; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    grdPieces.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                    grdPieces.RowDefinitions.Add(new RowDefinition { Height = new(1.1, GridUnitType.Star) });
                    grdPieces.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                }
                else
                    grdPieces.RowDefinitions.Add(new RowDefinition { Height = new(3.4, GridUnitType.Star) });
            }
            grdPieces.RowDefinitions.Add(new RowDefinition { Height = new(5, GridUnitType.Star) });
            //Initialize the pieces on the game UI/UX board
            for (int i = 1; i < 24; i++)
            {
                Row = CreateEmptyCenteredGrid();
                if (i % 2 != 0)
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < GetAmountOfColumns(i); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        BoardPieceButtons[i][k - 1] = CreateApexButton(k, i);
                        BoardPieceButtons[i][k - 1].Clicked += OnBuildOptionsButtonClicked;
                        Row.Add(BoardPieceButtons[i][k - 1], k);
                        BoardPieceImages[i][k - 1] = CreateApexImage(k, i);
                        BoardPieceImages[i][k - 1].Source = game.BoardPieces[(i - 1) * 12 + k - 1];
                        Row.Add(BoardPieceImages[i][k - 1], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    Row.ColumnSpacing = i == 7 || i == 9 || i == 11 || i == 13 || i == 15 || i == 17 ? 67 : 52;
                }
                else
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
                    for (int k = 1; k < GetAmountOfColumns(i); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                        BoardPieceButtons[i][k - 1] = CreateRoadButton(i % 4 == 0 ? 90 : k % 2 == 0 ? 30 : -30, k, i);
                        BoardPieceButtons[i][k - 1].Clicked += OnBuildOptionsButtonClicked;
                        Row.Add(BoardPieceButtons[i][k - 1], k);
                        BoardPieceImages[i][k - 1] = CreateRoadImage(i % 4 == 0 ? 90 : k % 2 == 0 ? 30 : -30, k, i);
                        BoardPieceImages[i][k - 1].Source = game.BoardPieces[(i - 1) * 12 + k - 1];
                        Row.Add(BoardPieceImages[i][k - 1], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
                    Row.ColumnSpacing = i % 4 == 0 ? i == 8 || i == 12 || i == 16 ? 67 : 45 : 12.5;
                    Row.Rotation = i > 12 ? 180 : 0;
                }
                grdPieces.Add(Row, 0, i);
            }
            //Show build options for the first player for starting turn
            if (game.PlayerIndicator == 0)
                ShowBuildOptions(Strings.Town);
            // Define the Rows In otherPieces  (for UI/UX layout purposes)
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(1, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.3, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.4, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.5, GridUnitType.Star) });
            otherPieces.RowSpacing = 5;
            Row = new()
            {
                WidthRequest = sizeProportion * 0.50,
                ColumnSpacing = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
            SKLottieView diceRoll = CreateDiceAnimation();
            Image diceImage = CreateDiceImage();
            for (int i = 0; i < 2; i++)
            {
                Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
                if (i == 0)
                {
                    Dice1Image = diceImage;
                    Dice1Roll = diceRoll;
                    Row.Add(Dice1Image, 1);
                    Row.Add(Dice1Roll, 1);
                    diceImage = CreateDiceImage();
                    diceRoll = CreateDiceAnimation();
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(2, GridUnitType.Star) });
                }
                else
                {
                    Dice2Image = diceImage;
                    Dice2Roll = diceRoll;
                    Row.Add(Dice2Image, 2);
                    Row.Add(Dice2Roll, 2);
                }
            }
            otherPieces.Add(Row);
            Label rollLabel = new()
            {
                Text = Strings.RollLabel,
                FontSize = 24,
                TextColor = Colors.DarkOrange,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            RollLabel = rollLabel;
            otherPieces.Add(RollLabel, 0, 1);
            Button rollButton = new()
            {
                Text = Strings.ButtonRoll,
                FontSize = 24,
                WidthRequest = sizeProportion * 0.35,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Colors.DodgerBlue,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            rollButton.Clicked += OnRollButtonClicked;
            RollButton = rollButton;
            otherPieces.Add(RollButton, 0, 2);
            Row = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            LongestRoad = new()
            {
                Source = Strings.LongestRoadImage,
                HeightRequest = sizeProportion * 0.2,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                Opacity = Keys.DoesNotOwn
            };
            Row.Add(LongestRoad);
            LargestArmy = new()
            {
                Source = Strings.LargestArmyImage,
                HeightRequest = sizeProportion * 0.2,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                Opacity = Keys.DoesNotOwn
            };
            Row.Add(LargestArmy, 1, 0);
            Row.ColumnSpacing = 5;
            otherPieces.Add(Row,0,3);
        }
        public override void ShowBuildOptions(string pieceType)
        {
            string[][] BoardPieces = new string[24][];
            for (int i = 1; i < 24; i++)
            {
                BoardPieces[i] = new string[GetAmountOfColumns(i) - 1];
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (game.BoardPieces[((i - 1) * 12) + k] != null)
                        BoardPieces[i][k] = game.BoardPieces[((i - 1) * 12) + k];
            }
            if (pieceType == Strings.Road || pieceType == Strings.All)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    {
                        if (i % 2 == 1)
                        {
                            VertexNode vertexNode = game.GameBoard.Vertices[GetPieceLocationInArray(i, k)];
                            if ((vertexNode.PieceType == BoardModel.PieceType.Town || vertexNode.PieceType == BoardModel.PieceType.City) && vertexNode.PlayerIndex == game.PlayerIndicator)
                            {
                                EdgeLink[] edges = vertexNode.Edges;
                                for (int j = 0; j < edges.Length; j++)
                                    if (edges[j].RoadOwnerPlayerIndex == -1)
                                        BoardPieceButtons[edges[j].Row][edges[j].Column].BorderWidth = Keys.ButtonVisible;
                            }
                        }
                        else
                        {
                            EdgeLink edge = game.GameBoard.Edges[GetPieceLocationInArray(i, k)];
                            if (edge.RoadOwnerPlayerIndex == game.PlayerIndicator)
                            {
                                //if vertex not owned by another player
                                if (edge.VertexNodeOne.PlayerIndex == -1 || edge.VertexNodeOne.PlayerIndex == game.PlayerIndicator)
                                {
                                    EdgeLink[] edges = edge.VertexNodeOne.Edges;
                                    for (int j = 0; j < edges.Length; j++)
                                        if (edges[j].RoadOwnerPlayerIndex == -1)
                                            BoardPieceButtons[edges[j].Row][edges[j].Column].BorderWidth = Keys.ButtonVisible;
                                }
                                if (edge.VertexNodeTwo.PlayerIndex == -1 || edge.VertexNodeTwo.PlayerIndex == game.PlayerIndicator)
                                {
                                    EdgeLink[] edges = edge.VertexNodeTwo.Edges;
                                    for (int j = 0; j < edges.Length; j++)
                                        if (edges[j].RoadOwnerPlayerIndex == -1)
                                            BoardPieceButtons[edges[j].Row][edges[j].Column].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                        }

                    }
            if (pieceType == Strings.All)
            {
                for (int i = 2; i < 24; i += 2)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    {
                        EdgeLink edge = game.GameBoard.Edges[GetPieceLocationInArray(i, k)];
                        if (i % 2 == 0 && edge.RoadOwnerPlayerIndex == game.PlayerIndicator)
                        {
                            if (edge.VertexNodeOne.PlayerIndex == -1)
                                BoardPieceButtons[edge.VertexNodeOne.Row][edge.VertexNodeOne.Column].BorderWidth = Keys.ButtonVisible;
                            if (edge.VertexNodeTwo.PlayerIndex == -1)
                                BoardPieceButtons[edge.VertexNodeTwo.Row][edge.VertexNodeTwo.Column].BorderWidth = Keys.ButtonVisible;
                        }
                    }
            }
            if (pieceType == Strings.City || pieceType == Strings.All)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPieces[i][k].Equals(GetPiecesColor(game.PlayerIndicator + 1) + Strings.Town.ToLower()))
                            BoardPieceButtons[i][k].BorderWidth = Keys.ButtonVisible;
            if (pieceType == Strings.Town)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPieces[i][k].Equals(string.Empty) && i % 2 == 1)
                            BoardPieceButtons[i][k].BorderWidth = Keys.ButtonVisible;

        }
    }
}
