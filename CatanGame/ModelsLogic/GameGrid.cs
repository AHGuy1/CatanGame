using CatanGame.Models;
using CatanGame.Views;
using SkiaSharp.Extended.UI.Controls;
using CommunityToolkit.Maui.Views;


namespace CatanGame.ModelsLogic
{
    public class GameGrid : GameGridModel
    {
        public GameGrid(Game game)
        {
            this.Game = game;
            BoardData = game.GameBoard;
            SpecialCards = new(this, Game, BoardData);
            for (int i = 1; i < 24; i++)
            {
                BoardPieceButtons[i] = new IndexedButton[GetAmountOfColumns(i) - 1];
                BoardPieceImages[i] = new Image[GetAmountOfColumns(i) - 1];
            }
            for (int i = 1;i < 6; i++)
                RobberImages[i-1] = new ImageButton[GetAmountOfColumnsTiles(i)];
        }
        public GameGrid() { }

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
        private static Grid CreateEmptyCenteredGrid()
        {
            return new()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
        }
        private static Grid CreateEmptyCardRowGrid()
        {
            double sizeProportion = GetSizeProportion();
            return new()
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start,
                ColumnSpacing = sizeProportion * 0.007
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
                WidthRequest = sizeProportion * 0.24,
                HeightRequest = sizeProportion * 0.24,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
        }
        private static Image CreateTileImage(string imageSource)
        {

            return new()
            {
                Source = imageSource,
                HeightRequest = GetSizeProportion() * 0.185,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
        }
        public static Image CreateCardImage(string source)
        {
            double sizeProportion = GetSizeProportion();
            return new()
            {
                Source = source,
                HeightRequest = sizeProportion * 0.158,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
            };
        }
        private static Image CreateDiceImage()
        {
            double sizeProportion = GetSizeProportion();
            return new()
            {
                Source = Strings.DiceSixImage,
                WidthRequest = sizeProportion * 0.14,
                HeightRequest = sizeProportion * 0.14,
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
        private static Image CreateRoadImage(int rotation)
        {
            return new()
            {
                HeightRequest = GetSizeProportion() * 0.0165,
                WidthRequest = GetSizeProportion() * 0.0495,
                HorizontalOptions = LayoutOptions.Center,
                Rotation = rotation,
            };
        }
        private static Image CreateApexImage()
        {
            return new()
            {
                HeightRequest = GetSizeProportion() * 0.03525,
                WidthRequest = GetSizeProportion() * 0.03525,
            };
        }
        private static Label CreateCardLabel()
        {
            return new()
            {
                FontSize = GetSizeProportion() * 0.05,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
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
         
        protected override void OnRobberPlacementClicked(object? sender, EventArgs e)
        { 
                IndexedImageButton? imageButton = (IndexedImageButton)sender!;
                if (imageButton != null && imageButton.BorderWidth == Keys.ButtonVisible)
                {
                    RobberImages[Game.RobberPlacment[0]][Game.RobberPlacment[1]].Source = null;
                    imageButton.Source = Strings.RobberImage;
                    BoardData.Hexes[GetTileLocationInArray(Game.RobberPlacment[0] + 1, Game.RobberPlacment[1] + 1)].HasRobber = false;
                    Game.RobberPlacment[0] = imageButton.RowIndex;
                    Game.RobberPlacment[1] = imageButton.ColumnIndex;
                    BoardData.Hexes[GetTileLocationInArray(Game.RobberPlacment[0] + 1, Game.RobberPlacment[1] + 1)].HasRobber = true;
                    Dictionary<string, object> dict = new()
                    {
                        { nameof(Game.RobberPlacment), Game.RobberPlacment }
                    };
                    Game.UpdateFields(dict);
                    HideRobberButtuns();
                }
        }
        protected override void ShowBuildOptions()
        {
            ShowBuildOptions(Strings.All);
        }
        protected override void ShowRobberPlacmentOptions()
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                for (int i = 0; i < 5; i++)
                    for (int k = 0; k < GetAmountOfColumnsTiles(i + 1); k++)
                        if (!BoardData.Hexes[GetTileLocationInArray(i + 1, k + 1)].HasRobber && RobberImages[i][k] != null)
                            SetVisibleRobberImages(i, k);
            });
        }
        protected override void HideRobberButtuns()
        {
            for (int i = 0; i < 5; i++)
                for (int k = 0; k < GetAmountOfColumnsTiles(i + 1); k++)
                    if (RobberImages[i][k].BorderWidth == Keys.ButtonVisible)
                        RobberImages[i][k].BorderWidth = 0;
        }
        protected override void HideButtuns()
        {
            for (int i = 1; i < 24; i++)
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (BoardPieceButtons[i][k].BorderWidth == Keys.ButtonVisible)
                        BoardPieceButtons[i][k].BorderWidth = 0;
        }
        protected override void OnBuildButtonClicked(object? sender, EventArgs e)
        {
            IndexedButton? button = (IndexedButton)sender!;
            if (button.BorderWidth == Keys.ButtonVisible)
            {
                if (button.RowIndex % 2 == 0 && ImageSource.IsNullOrEmpty(BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                {
                    BuildRoad(button.RowIndex, button.ColumnIndex - 1);
                    if(SpecialCards.RoadBuildingStuatus == SpecialCardsModel.RoadBuilding.First)
                    {
                        SpecialCards.RoadBuildingStuatus = SpecialCardsModel.RoadBuilding.Second;
                        HideButtuns();
                        ShowBuildOptions(Strings.Road);
                    }
                    else
                    {
                        if (SpecialCards.RoadBuildingStuatus == SpecialCardsModel.RoadBuilding.Second)
                            SpecialCards.RoadBuildingStuatus = SpecialCardsModel.RoadBuilding.Disabled;
                        else if (Game.Turn > Game.PlayerCount * 2)
                        {
                            Game.PlayerBrickCount--;
                            Game.PlayerWoodCount--;
                        }
                        HideButtuns();
                    }
                }
                else
                {
                    if (button.RowIndex % 2 == 1 && ImageSource.IsNullOrEmpty(BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source))
                    {
                        BuildTown(button.RowIndex, button.ColumnIndex - 1);
                        if(Game.Turn > Game.PlayerCount * 2)
                        {
                            Game.PlayerBrickCount--;
                            Game.PlayerWheatCount--;
                            Game.PlayerSheepCount--;
                            Game.PlayerWoodCount--;
                        }
                    }
                    else if (button.RowIndex % 2 == 1 && BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()!.Contains(GetPiecesColor(Game.PlayerIndicator + 1) + Strings.Town.ToLower()))
                    {
                        BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source = (GetPiecesColor(Game.PlayerIndicator + 1) + Strings.City).ToLower();
                        Game.BoardPieces[((button.RowIndex - 1) * 12) + (button.ColumnIndex - 1)] = BoardPieceImages[button.RowIndex][button.ColumnIndex - 1].Source.ToString()![6..];
                        Game.PlayerTownCount--;
                        Game.PlayerCityCount++;
                        BoardData.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PlayerIndex = Game.PlayerIndicator;
                        BoardData.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PieceType = BoardModel.PieceType.City;
                        Game.PlayerOreCount -= 3;
                        Game.PlayerWheatCount -= 2;
                    }
                    UpdateBoardPices();
                    UpdateResourceCounters();
                    HideButtuns();
                }
                //if just built a town and is one of the first 2 turns show road building options
                if (Game.Turn <= Game.PlayerCount * 2 && button.RowIndex % 2 == 1 && BoardData.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PlayerIndex == Game.PlayerIndicator && BoardData.Vertices[GetPieceLocationInArray(button.RowIndex, button.ColumnIndex - 1)].PieceType == BoardModel.PieceType.Town)
                    ShowBuildOptions(Strings.Road);
            }
        }
        protected override void CheckLongestRoad()
        {
            EdgeLink[] edges = BoardData.Edges;
            int playerLongestRoad = 0;
            for (int i = 0; i < edges.Length; i++)
            {
                if (edges[i].RoadOwnerPlayerIndex == Game.PlayerIndicator)
                {
                    bool[] visited = new bool[edges.Length];
                    int roadLength = CheckLongestRoad(edges[i], visited);
                    if (roadLength > playerLongestRoad)
                        playerLongestRoad = roadLength;
                }
            }
            Game.PlayerLongestRoadLength = playerLongestRoad;
            if (Game.PlayerLongestRoadLength > Game.LongestRoadLength)
            {
                LongestRoad.Opacity = 1;
                Game.LongestRoadLength = Game.PlayerLongestRoadLength;
                Game.LongestRoadOwnerIndex = Game.PlayerIndicator;
            }
            else if(LongestRoad.Opacity == 1)
            {
                Game.LongestRoadLength = Game.PlayerLongestRoadLength;
                if(Game.LongestRoadLength < 5)
                    LongestRoad.Opacity = Keys.DoesNotOwn;
            }
            Dictionary<string, object> dict = new()
            {
                { nameof(Game.BoardPieces), Game.BoardPieces },
                { nameof(Game.LongestRoadLength), Game.LongestRoadLength }
            };
            Game.UpdateFields(dict);
        }
        protected override void OnRollButtonClicked(object? sender, EventArgs e)
        {
            RollDice();
        }
        protected override void RollDice()
        {
            RollButton.IsEnabled = false;
            Random random = new();
            Game.Roll1 = random.Next(1, 7);
            Game.Roll2 = random.Next(1, 7);
            Game.IsRolling = true;
            StartAnimations();
            Dictionary<string, object> dict = new()
            {
                { nameof(Game.Roll1), Game.Roll1 },
                { nameof(Game.Roll2), Game.Roll2 },
                { nameof(Game.IsRolling), Game.IsRolling }
            };
            Game.UpdateFields(OnDiceUpdated, dict);
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
                Dice1Image.Source = GetDiceImage(Game.Roll1);
                Dice1Image.IsVisible = true;
                Dice2Roll.IsAnimationEnabled = false;
                Dice2Image.Source = GetDiceImage(Game.Roll2);
                Dice2Image.IsVisible = true;
                Dice2Roll.IsVisible = false;
                RollLabel.Text = Strings.Rolled + Game.RollTotal;
            });
        }
        protected override void EndTurn()
        {
            EndTurnOnClicked?.Invoke(this, EventArgs.Empty);
        }
        protected override void BuildTownAtFirstPosition()
        {
            bool foundPlaceToBuild = false;
            for (int i = 1; i < 24 && !foundPlaceToBuild; i += 2)
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (BoardPieceButtons[i][k].BorderWidth == Keys.ButtonVisible)
                    {
                        if (BoardData.Vertices[GetPieceLocationInArray(i, k)].PieceType == BoardModel.PieceType.None)
                        {
                            BuildTown(i, k);
                            UpdateBoardPices();
                            HideButtuns();
                            ShowBuildOptions(Strings.Road);
                            foundPlaceToBuild = true;
                        }
                    }
        }
        protected override void BuildTown(int row, int column)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                BoardPieceImages[row][column].Source = (GetPiecesColor(Game.PlayerIndicator + 1) + Strings.Town).ToLower();
                Game.BoardPieces[((row - 1) * 12) + column] = BoardPieceImages[row][column].Source.ToString()![6..];
                Game.PlayerTownCount++;
                BoardData.Vertices[GetPieceLocationInArray(row, column)].PlayerIndex = Game.PlayerIndicator;
                BoardData.Vertices[GetPieceLocationInArray(row, column)].PieceType = BoardModel.PieceType.Town;
                CheckIfOnHarbor(row, column);
            });
        }
        protected override void CheckIfOnHarbor(int row, int column)
        {
            int harborType;
            harborType = (row, column) switch
            {
                (1, 0) or (3, 0) or (11, 5) or (11, 5) or (21, 0) or (21, 2) or (23, 0) or (23, 1) => 0,
                (7, 0) or (9, 0) => 1,
                (15, 0) or (17, 0) => 2,
                (17, 4) or (19, 3) => 3,
                (1, 1) or (3, 2) => 4,
                (5, 3) or (7, 4) => 5,
                //No harbor
                _ => -1,
            };
            if (harborType != -1)
                Game.PlayerOwnedHarbors[harborType] = true;
        }
        protected override void BuildRoadAtFirstPosition()
        {
            bool foundPlaceToBuild = false;
            for (int i = 2; i < 24 && !foundPlaceToBuild; i += 2)
                for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    if (BoardPieceButtons[i][k].BorderWidth == Keys.ButtonVisible)
                    {
                        if (BoardData.Edges[GetPieceLocationInArray(i, k)].RoadOwnerPlayerIndex == -1)
                        {
                            BuildRoad(i, k);
                            UpdateBoardPices();
                            HideButtuns();
                            foundPlaceToBuild = true;
                        }
                    }
        }
        protected override void BuildRoad(int row, int column)
        {
            BoardPieceImages[row][column].Source = (GetPiecesColor(Game.PlayerIndicator + 1) + Strings.Road).ToLower();
            Game.BoardPieces[((row - 1) * 12) + column] = BoardPieceImages[row][column].Source.ToString()![6..];
            BoardData.Edges[GetPieceLocationInArray(row, column)].RoadOwnerPlayerIndex = GetPieceIndexFromColor(row, column);
            Game.PlayerRoadCount++;
            CheckLongestRoad();
        }
        protected override void UpdateBoardPices()
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(Game.BoardPieces), Game.BoardPieces }
            };
            Game.UpdateFields(dict);
        }
        protected override void Trade()
        {
            if (CurrentTradePopUp != null)
                CloseTradePopUp();
            CurrentTradePopUp = new(Game);
            CurrentGamePage?.ShowPopup(CurrentTradePopUp);
        }
        protected override void UseCard(object paramter)
        {
            if(paramter is string source)
            {
                if (source == Strings.KnightImage)
                {
                    SpecialCards.UseKnight();
                    Game.PlayerLargestArmySize++;
                    if(Game.PlayerLargestArmySize > Game.LargestArmySize)
                    {
                        LargestArmy.Opacity = 1;
                        Game.LargestArmySize = Game.PlayerLargestArmySize;
                        Dictionary<string, object> dict = new()
                        {
                            {nameof(Game.LargestArmySize),Game.LargestArmySize }
                        };
                        Game.UpdateFields(dict);
                    }
                }
                else if(source == Strings.RoadBuildingImage)
                    SpecialCards.UseRoadBuilding();
                else if (source == Strings.MonopolyImage)
                    SpecialCards.UseMonopoly();
                else if(source == Strings.YearOfPlentyImage)
                    SpecialCards.UseYearOfPlenty();
            }
            UpdateSpecialCards();
        }
        protected override void UpdateSpecialCards()
        {
            Counters[5].Text = SpecialCards.PlayerKnightCount.ToString();
            (SpecialCardImages[0].Command as Command)?.ChangeCanExecute();
            Counters[6].Text = SpecialCards.PlayerUniversityCount.ToString();
            (SpecialCardImages[1].Command as Command)?.ChangeCanExecute();
            Counters[7].Text = SpecialCards.PlayerRoadBuildingCount.ToString();
            (SpecialCardImages[2].Command as Command)?.ChangeCanExecute();
            Counters[8].Text = SpecialCards.PlayerMonopolyCount.ToString();
            (SpecialCardImages[3].Command as Command)?.ChangeCanExecute();
            Counters[9].Text = SpecialCards.PlayerYearOfPlentyCount.ToString();
            (SpecialCardImages[4].Command as Command)?.ChangeCanExecute();
        }
        protected override void GetCardFromPackege()
        {
            Game.PlayerSheepCount--;
            Game.PlayerOreCount--;
            Game.PlayerWheatCount--;
            UpdateResourceCounters();
            SpecialCards.GetCardFromPackege();
            UpdateSpecialCards();
        }
        protected override async void OnDiceUpdated(Task task)
        {
            await Task.Delay(2000);
            if (task.IsCompletedSuccessfully)
            {
                StopAnimations();
                if (Game.RollTotal == 7)
                    ShowRobberPlacmentOptions();
                else
                {
                    Game.AllocateResources();
                    UpdateResourceCounters();
                }
                Game.IsRolling = false;
                Dictionary<string, object> dict = new()
                {
                    { nameof(Game.IsRolling), Game.IsRolling }
                };
                Game.UpdateFields(dict);
            }
        }
        protected override BoardModel.PieceType GetPieceType(int row, int column)
        {
            return BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.Town, StringComparison.CurrentCultureIgnoreCase) ? BoardModel.PieceType.Town :
                   BoardPieceImages[row][column].Source.ToString()!.Contains(Strings.City, StringComparison.CurrentCultureIgnoreCase) ? BoardModel.PieceType.City :
                   BoardModel.PieceType.None;
        }
        protected override ImageButton CreateCardImageButton(string source)
        {
            double sizeProportion = GetSizeProportion();
            return new()
            {
                Source = source,
                HeightRequest = sizeProportion * 0.15,
                WidthRequest = sizeProportion * 0.1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Command = new Command(UseCard, CenUseCard),
                CommandParameter = source
            };
        }
        protected override Grid CreateRobberImage(int row, int column)
        {
            Grid grid = CreateEmptyCenteredGrid();
            grid.RowSpacing = GetSizeProportion() * 0.05;
            grid.RowDefinitions.Add(new RowDefinition { Height = new(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new(1, GridUnitType.Star) });
            IndexedImageButton imageButton = new(row, column, GameGrid.GetSizeProportion() * 0.055);
            if (row == Game.RobberPlacment[0] && column == Game.RobberPlacment[1])
            {
                imageButton.Source = Strings.RobberImage;
            }
            RobberImages[row][column] = imageButton;
            RobberImages[row][column].Clicked += OnRobberPlacementClicked;
            grid.Add(imageButton, 0, 1);
            return grid;
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
        protected override int CheckLongestRoad(EdgeLink edge, bool[] visited)
        {
            if (visited[GetPieceLocationInArray(edge.Row, edge.Column)])
                return 0;
            int longestBranch = 0;
            int curentBranch;
            visited[GetPieceLocationInArray(edge.Row, edge.Column)] = true;
            //if vertex not owned by another player
            if (edge.VertexNodeOne.PlayerIndex == -1 || edge.VertexNodeOne.PlayerIndex == Game.PlayerIndicator)
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
                            if (edges[i].RoadOwnerPlayerIndex == Game.PlayerIndicator)
                            {
                                curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                                if (curentBranch > longestBranch)
                                    longestBranch = curentBranch;
                            }
                }
                else
                    for (int i = 0; i < 2; i++)
                        if (edges[i].RoadOwnerPlayerIndex == Game.PlayerIndicator)
                        {
                            curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                            if (curentBranch > longestBranch)
                                longestBranch = curentBranch;
                        }
            }
            if (edge.VertexNodeTwo.PlayerIndex == -1 || edge.VertexNodeTwo.PlayerIndex == Game.PlayerIndicator)
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
                            if (edges[i].RoadOwnerPlayerIndex == Game.PlayerIndicator)
                            {
                                curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                                if (curentBranch > longestBranch)
                                    longestBranch = curentBranch;
                            }
                }
                else
                    for (int i = 0; i < 2; i++)
                        if (edges[i].RoadOwnerPlayerIndex == Game.PlayerIndicator)
                        {
                            curentBranch = CheckLongestRoad(edges[i], visited) + 1;
                            if (curentBranch > longestBranch)
                                longestBranch = curentBranch;
                        }
            }
            visited[GetPieceLocationInArray(edge.Row, edge.Column)] = false;
            return longestBranch;
        }   
        protected override bool CanShowBuildOptions()
        {
            return Game.StatusMessage == Strings.YourTurn;
        }
        protected override bool CanEndTurn()
        {
            return Game.PlayerIndicator + 1 == Game.PlayerTurn && Game.IsFull && ((Game.Turn <= Game.PlayerCount * 2 && Game.PlayerRoadCount >= (double)(Game.Turn / Game.PlayerCount)) || (Game.Turn > Game.PlayerCount * 2 && !Game.IsRolling && !RollButton.IsEnabled));
        }
        protected override bool CenTrade()
        {
            return Game.PlayerTurn == Game.PlayerIndicator + 1;
        }
        protected override bool CenGetCardFromPackege()
        {
            return Game.PlayerSheepCount > 0 && Game.PlayerOreCount > 0 && Game.PlayerWheatCount > 0 && !String.IsNullOrWhiteSpace(SpecialCards.CardPack[0]);
        }
        protected override bool CenUseCard(object paramter)
        {
            if(paramter is string source)
            {
                if(source == Strings.KnightImage)
                    return SpecialCards.PlayerKnightCount > 0;
                else if (source == Strings.RoadBuildingImage)
                    return SpecialCards.PlayerRoadBuildingCount > 0;
                else if (source == Strings.MonopolyImage)
                    return SpecialCards.PlayerMonopolyCount > 0;
                else if (source == Strings.YearOfPlentyImage)
                    return SpecialCards.PlayerYearOfPlentyCount > 0;
            }
            //wont happan
            return false;
        }
        public override void CloseTradePopUp()
        {
            if (CurrentTradePopUp != null)
                MainThread.BeginInvokeOnMainThread(() => CurrentTradePopUp.Close());
            CurrentTradePopUp = null;
        }
        public override void SetVisibleRobberImages(int row, int column)
        {
            RobberImages[row][column].BorderWidth = Keys.ButtonVisible;
        }
        public override void UpdateResourceCounters()
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                (SpecialCardImages[5].Command as Command)?.ChangeCanExecute();
                if (Counters[0] != null && Counters[0].Text != Game.PlayerWoodCount.ToString())
                    Counters[0].Text = Game.PlayerWoodCount.ToString();
                if (Counters[1] != null && Counters[1].Text != Game.PlayerBrickCount.ToString())
                    Counters[1].Text = Game.PlayerBrickCount.ToString();
                if (Counters[2] != null && Counters[2].Text != Game.PlayerSheepCount.ToString())
                    Counters[2].Text = Game.PlayerSheepCount.ToString();
                if (Counters[3] != null && Counters[3].Text != Game.PlayerWheatCount.ToString())
                    Counters[3].Text = Game.PlayerWheatCount.ToString();
                if (Counters[4] != null && Counters[4].Text != Game.PlayerOreCount.ToString())
                    Counters[4].Text = Game.PlayerOreCount.ToString();
            });

        }
        public override void OnAnimationStatusChanged()
        {
            if (Game.IsRolling)
                StartAnimations();
            else
            {
                if(Game.RollTotal != 7)
                {
                    Game.AllocateResources();
                    UpdateResourceCounters();
                }
                StopAnimations();
            }
        }
        public override void OnChange()
        {
            if (Game.BoardPieces != null && BoardPieceImages != null && BoardPieceButtons != null)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                        if (BoardPieceImages[i][k].Source != null && Game.BoardPieces[((i - 1) * 12) + k] != null && BoardPieceImages[i][k].Source.ToString()![6..] != Game.BoardPieces[((i - 1) * 12) + k])
                        {
                            BoardPieceImages[i][k].Source = Game.BoardPieces[((i - 1) * 12) + k];
                            if (i % 2 == 0)
                                BoardData.Edges[GetPieceLocationInArray(i, k)].RoadOwnerPlayerIndex = GetPieceIndexFromColor(i, k);
                            else
                            {
                                VertexNode vertex = BoardData.Vertices[GetPieceLocationInArray(i, k)];
                                vertex.PlayerIndex = GetPieceIndexFromColor(i, k);
                                vertex.PieceType = GetPieceType(i, k);
                                if(vertex.PieceType == BoardModel.PieceType.Town)
                                    CheckLongestRoad();
                            }
                        }
            for (int i = 0; i < 5; i++)
                for (int k = 0; k < GetAmountOfColumnsTiles(i + 1); k++)
                    if (BoardData.Hexes[GetTileLocationInArray(i + 1,k + 1)].HasRobber &&( i != Game.RobberPlacment[0] || k != Game.RobberPlacment[1]))
                    {
                        RobberImages[i][k].Source = null;
                        BoardData.Hexes[GetTileLocationInArray(i + 1, k + 1)].HasRobber = false;
                    }
                    else if (i == Game.RobberPlacment[0] && k == Game.RobberPlacment[1])
                    {
                        RobberImages[i][k].Source = Strings.RobberImage;
                        BoardData.Hexes[GetTileLocationInArray(i + 1, k + 1)].HasRobber = true;
                    }
            if (Game.PlayerLongestRoadLength > Game.LongestRoadLength)
            {
                LongestRoad.Opacity = 1;
                Game.LongestRoadLength = Game.PlayerLongestRoadLength;
                Dictionary<string, object> dict = new()
                {
                    { nameof(Game.LongestRoadLength), Game.LongestRoadLength }
                };
                Game.UpdateFields(dict);
            }
            if (LongestRoad.Opacity != Keys.DoesNotOwn && Game.PlayerLongestRoadLength < Game.LongestRoadLength)
                LongestRoad.Opacity = Keys.DoesNotOwn;
            if (LargestArmy.Opacity != Keys.DoesNotOwn && Game.PlayerLargestArmySize < Game.LargestArmySize)
                LargestArmy.Opacity = Keys.DoesNotOwn;
            (ShowBuildOptionsCommand as Command)?.ChangeCanExecute();
            (EndTurnCommand as Command)?.ChangeCanExecute();
            UpdateResourceCounters();
        }
        public override async void EnsurePlayerPlayed()
        {
            if (RollButton.IsEnabled)
            {
                RollDice();
                await Task.Delay(3000);
            }
            else if (Game.IsRolling)
                await Task.Delay(2000);
            if (Game.Turn <= Game.PlayerCount * 2)
            {
                if(Game.PlayerTownCount <= Game.Turn / Game.PlayerCount)
                {
                    BuildTownAtFirstPosition();
                    await Task.Delay(2000);
                }
                if (Game.PlayerRoadCount <= Game.Turn / Game.PlayerCount)
                {
                    BuildRoadAtFirstPosition();
                    await Task.Delay(2000);
                }
            }
            if(Game.TradeInProgress)
            {
                Game.CancelTradeRequest();
                await Task.Delay(2000);
            }
            Game.EndTurn();
        }
        public override void Init(Grid gameBoard, Grid grdPieces, Grid otherPieces, Image frame, GamePage gamePage)
        {
            CurrentGamePage = gamePage;
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
            //Initialize the tiles on the UI/UX Game board
            if (Game.PlayerIndicator != 0)
                for (int i = 1; i < 6; i++)
                {
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                    for (int k = 1; k < 1 + GetAmountOfColumnsTiles(i); k++)
                    {
                        Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(2, GridUnitType.Star) });
                        Row.Add(CreateTileImage(Game.TileTypes[GetTileLocationInArray(i, k)]), k);
                        Row.Add(CreateNumberImage(Game.TileNumbers[GetTileLocationInArray(i, k)]), k);
                        Row.Add(CreateRobberImage(i - 1, k - 1), k);
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
                        if (Game.IsRandomBoard)
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
                            GetFixedTile(i, k, out sourceTile, out sourceNumber);
                        Game.TileTypes[GetTileLocationInArray(i, k)] = sourceTile;
                        Row.Add(CreateTileImage(sourceTile), k);
                        Game.TileNumbers[GetTileLocationInArray(i, k)] = sourceNumber;
                        Row.Add(CreateNumberImage(sourceNumber), k);
                        if (sourceTile == Strings.Desert)
                        {
                            Game.RobberPlacment[0] = i - 1;
                            Game.RobberPlacment[1] = k - 1;
                        }
                        Row.Add(CreateRobberImage(i - 1, k - 1), k);
                    }
                    gameBoard.Add(Row, 0, i);
                    Row = CreateEmptyCenteredGrid();
                }
                Dictionary<string, object> dict = new()
                {
                    {nameof(Game.TileNumbers), Game.TileNumbers},
                    {nameof(Game.TileTypes), Game.TileTypes},
                    {nameof(Game.RobberPlacment), Game.RobberPlacment}
                };
                //Update the firebase with the new tile types and numbers
                Game.UpdateFields(dict);
            }
            //Connect the Game logic board with the UI/UX board
            BoardData.InitBoard(BoardPieceButtons, Game.TileTypes, Game.TileNumbers);
            BoardData.Hexes[GetTileLocationInArray(Game.RobberPlacment[0] + 1, Game.RobberPlacment[1] + 1)].HasRobber = true;
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
            //Initialize the pieces on the Game UI/UX board
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
                        BoardPieceButtons[i][k - 1].Clicked += OnBuildButtonClicked;
                        Row.Add(BoardPieceButtons[i][k - 1], k);
                        BoardPieceImages[i][k - 1] = CreateApexImage();
                        BoardPieceImages[i][k - 1].Source = Game.BoardPieces[(i - 1) * 12 + k - 1];
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
                        BoardPieceButtons[i][k - 1].Clicked += OnBuildButtonClicked;
                        Row.Add(BoardPieceButtons[i][k - 1], k);
                        BoardPieceImages[i][k - 1] = CreateRoadImage(i % 4 == 0 ? 90 : k % 2 == 0 ? 30 : -30);
                        BoardPieceImages[i][k - 1].Source = Game.BoardPieces[(i - 1) * 12 + k - 1];
                        Row.Add(BoardPieceImages[i][k - 1], k);
                    }
                    Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
                    Row.ColumnSpacing = i % 4 == 0 ? i == 8 || i == 12 || i == 16 ? 67 : 45 : 12.5;
                    Row.Rotation = i > 12 ? 180 : 0;
                }
                grdPieces.Add(Row, 0, i);
            }
            //Show build options for the first player for starting turn
            if (Game.PlayerIndicator == 0)
                ShowBuildOptions(Strings.Town);
            // Define the Rows In otherPieces  (for UI/UX layout purposes)
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(1, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.31, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.48, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.48, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.48, GridUnitType.Star) });
            otherPieces.RowDefinitions.Add(new RowDefinition { Height = new(0.85, GridUnitType.Star) });
            otherPieces.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            otherPieces.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            otherPieces.ColumnSpacing = 4.5;
            otherPieces.RowSpacing = 5;
            Row = new()
            {
                WidthRequest = sizeProportion * 0.41,
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
                FontSize = 22,
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
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = sizeProportion * 0.28,
                BackgroundColor = Colors.DodgerBlue,
                TextColor = Colors.White,
                IsEnabled = false,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            rollButton.Clicked += OnRollButtonClicked;
            RollButton = rollButton;
            otherPieces.Add(RollButton, 0, 2);
            ShowBuildOptionsCommand = new Command(ShowBuildOptions, CanShowBuildOptions);
            Button buildOptionsButton = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = Strings.BuildOptions,
                FontSize = 18,
                WidthRequest = sizeProportion * 0.35,
                Command = ShowBuildOptionsCommand
            };
            otherPieces.Add(buildOptionsButton, 0, 3);
            EndTurnCommand = new Command(EndTurn, CanEndTurn);
            Button endTurnButton = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = sizeProportion * 0.28,
                FontSize = 18,
                Text = Strings.EndTurn,
                Command = EndTurnCommand
            };
            otherPieces.Add(endTurnButton, 0, 4);
            Row = CreateEmptyCardRowGrid();
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            LongestRoad = CreateCardImage(Strings.LongestRoadImage);
            LongestRoad.Opacity = Keys.DoesNotOwn;
            Row.Add(LongestRoad);
            LargestArmy = CreateCardImage(Strings.LargestArmyImage);
            LargestArmy.Opacity = Keys.DoesNotOwn;
            Row.Add(LargestArmy, 1);
            Row.Add(CreateCardImage(Strings.BuildingCostImage), 2);
            otherPieces.Add(Row, 0, 5);
            Row = CreateEmptyCardRowGrid();
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.Add(CreateCardImage(Strings.WoodImage));
            Row.Add(CreateCardImage(Strings.BrickImage), 1);
            Row.Add(CreateCardImage(Strings.SheepImage), 2);
            Row.Add(CreateCardImage(Strings.WheatImage), 3);
            Row.Add(CreateCardImage(Strings.OreImage), 4);
            Counters[0] = CreateCardLabel();
            Counters[0].Text = Game.PlayerWoodCount.ToString();
            Row.Add(Counters[0]);
            Counters[1] = CreateCardLabel();
            Counters[1].Text = Game.PlayerBrickCount.ToString();
            Row.Add(Counters[1], 1);
            Counters[2] = CreateCardLabel();
            Counters[2].Text = Game.PlayerSheepCount.ToString();
            Row.Add(Counters[2], 2);
            Counters[3] = CreateCardLabel();
            Counters[3].Text = Game.PlayerWheatCount.ToString();
            Row.Add(Counters[3], 3);
            Counters[4] = CreateCardLabel();
            Counters[4].Text = Game.PlayerOreCount.ToString();
            Row.Add(Counters[4], 4);
            otherPieces.Add(Row, 1, 5);
            Row = new Grid()
            {
                ColumnSpacing = sizeProportion * 0.007,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start
            };
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            Row.ColumnDefinitions.Add(new ColumnDefinition { Width = new(1, GridUnitType.Star) });
            SpecialCardImages[5] = new()
            {
                Source = Strings.CardBackGround,
                HeightRequest = sizeProportion * 0.15,
                WidthRequest = sizeProportion * 0.1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Command = new Command(GetCardFromPackege, CenGetCardFromPackege),
            };
            Row.Add(SpecialCardImages[5]);
            SpecialCardImages[0] = CreateCardImageButton(Strings.KnightImage);
            Row.Add(SpecialCardImages[0], 1);
            SpecialCardImages[1] = CreateCardImageButton(Strings.UniversityImage);
            Row.Add(SpecialCardImages[1], 2);
            SpecialCardImages[2] = CreateCardImageButton(Strings.RoadBuildingImage);
            Row.Add(SpecialCardImages[2], 3);
            SpecialCardImages[3] = CreateCardImageButton(Strings.MonopolyImage);
            Row.Add(SpecialCardImages[3], 4);
            SpecialCardImages[4] = CreateCardImageButton(Strings.YearOfPlentyImage);
            Row.Add(SpecialCardImages[4], 5);
            Counters[5] = CreateCardLabel();
            Counters[5].Text = SpecialCards.PlayerKnightCount.ToString();
            Row.Add(Counters[5], 1);
            Counters[6] = CreateCardLabel();
            Counters[6].Text = SpecialCards.PlayerUniversityCount.ToString();
            Row.Add(Counters[6], 2);
            Counters[7] = CreateCardLabel();
            Counters[7].Text = SpecialCards.PlayerRoadBuildingCount.ToString();
            Row.Add(Counters[7], 3);
            Counters[8] = CreateCardLabel();
            Counters[8].Text = SpecialCards.PlayerMonopolyCount.ToString();
            Row.Add(Counters[8], 4);
            Counters[9] = CreateCardLabel();
            Counters[9].Text = SpecialCards.PlayerYearOfPlentyCount.ToString();
            Row.Add(Counters[9], 5);
            otherPieces.Add(Row, 0, 4);
            Grid.SetColumnSpan(Row, 2);
            TradeButton = new()
            {
                Text = Strings.Trade,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = sizeProportion * 0.055,
                HeightRequest = sizeProportion * 0.12,
                WidthRequest = sizeProportion * 0.37,
                FontAttributes = FontAttributes.Bold,
                Command = new Command(Trade,CenTrade)
            };
            otherPieces.Add(TradeButton, 1, 3);
        }
        public override void ShowBuildOptions(string pieceType)
        {
            //Shows all options for building a road, if the player has the resources to build it, or if the player just built a town as part of the first two turns
            if (pieceType == Strings.Road || (pieceType == Strings.All && Game.PlayerWoodCount >= 1 && Game.PlayerBrickCount >= 1))
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    {
                        if (i % 2 == 1)
                        {
                            VertexNode vertexNode = BoardData.Vertices[GetPieceLocationInArray(i, k)];
                            if ((vertexNode.PieceType == BoardModel.PieceType.Town || vertexNode.PieceType == BoardModel.PieceType.City) && vertexNode.PlayerIndex == Game.PlayerIndicator)
                            {
                                EdgeLink[] edges = vertexNode.Edges;
                                for (int j = 0; j < edges.Length; j++)
                                    if (edges[j].RoadOwnerPlayerIndex == -1)
                                        BoardPieceButtons[edges[j].Row][edges[j].Column].BorderWidth = Keys.ButtonVisible;
                            }
                        }
                        else
                        {
                            EdgeLink edge = BoardData.Edges[GetPieceLocationInArray(i, k)];
                            if (edge.RoadOwnerPlayerIndex == Game.PlayerIndicator)
                            {
                                //if vertex not owned by another player
                                if (edge.VertexNodeOne.PlayerIndex == -1 || edge.VertexNodeOne.PlayerIndex == Game.PlayerIndicator)
                                {
                                    EdgeLink[] edges = edge.VertexNodeOne.Edges;
                                    for (int j = 0; j < edges.Length; j++)
                                        if (edges[j].RoadOwnerPlayerIndex == -1)
                                            BoardPieceButtons[edges[j].Row][edges[j].Column].BorderWidth = Keys.ButtonVisible;
                                }
                                if (edge.VertexNodeTwo.PlayerIndex == -1 || edge.VertexNodeTwo.PlayerIndex == Game.PlayerIndicator)
                                {
                                    EdgeLink[] edges = edge.VertexNodeTwo.Edges;
                                    for (int j = 0; j < edges.Length; j++)
                                        if (edges[j].RoadOwnerPlayerIndex == -1)
                                            BoardPieceButtons[edges[j].Row][edges[j].Column].BorderWidth = Keys.ButtonVisible;
                                }
                            }
                        }

                    }
            //Shows all options for buiding a town, if the player has the resources to build it, if there are any
            if (pieceType == Strings.All && Game.PlayerWoodCount >= 1 && Game.PlayerBrickCount >= 1 && Game.PlayerSheepCount >= 1 && Game.PlayerWheatCount >= 1)
            {
                for (int i = 2; i < 24; i += 2)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    {
                        EdgeLink edge = BoardData.Edges[GetPieceLocationInArray(i, k)];
                        if (i % 2 == 0 && edge.RoadOwnerPlayerIndex == Game.PlayerIndicator)
                        {
                            if (edge.VertexNodeOne.PlayerIndex == -1)
                            {
                                bool cenBuild = true;
                                EdgeLink[] edges = edge.VertexNodeOne.Edges;
                                for (int j = 0; j < edges.Length; j++)
                                    if (edges[j].VertexNodeOne.PlayerIndex != -1 || edges[j].VertexNodeTwo.PlayerIndex != -1)
                                        cenBuild = false;
                                if(cenBuild)
                                    BoardPieceButtons[edge.VertexNodeOne.Row][edge.VertexNodeOne.Column].BorderWidth = Keys.ButtonVisible;
                            }
                            if (edge.VertexNodeTwo.PlayerIndex == -1)
                            {
                                bool cenBuild = true;
                                EdgeLink[] edges = edge.VertexNodeTwo.Edges;
                                for (int j = 0; j < edges.Length; j++)
                                    if (edges[j].VertexNodeOne.PlayerIndex != -1 || edges[j].VertexNodeTwo.PlayerIndex != -1)
                                        cenBuild = false;
                                if (cenBuild)
                                    BoardPieceButtons[edge.VertexNodeTwo.Row][edge.VertexNodeTwo.Column].BorderWidth = Keys.ButtonVisible;
                            }
                        }
                    }
            }
            //Shows all options for buiding a town, if its one of the first two turns of the Game for said player
            if (pieceType == Strings.Town)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    {
                        if (i % 2 == 1)
                        {
                            VertexNode vertexNode = BoardData.Vertices[GetPieceLocationInArray(i, k)];
                            if (vertexNode.PlayerIndex == -1)
                            {
                                bool cenBuild = true;
                                EdgeLink[] edges = vertexNode.Edges;
                                for (int j = 0; j < edges.Length; j++)
                                    if (edges[j].VertexNodeOne.PlayerIndex != -1 || edges[j].VertexNodeTwo.PlayerIndex != -1)
                                        cenBuild = false;
                                if (cenBuild)
                                    BoardPieceButtons[i][k].BorderWidth = Keys.ButtonVisible;
                            }
                        }
                    }
            //Shows all options for building a city, if the player has the resources to build it, if there are any
            if (pieceType == Strings.All && Game.PlayerWheatCount >= 3 && Game.PlayerOreCount >= 2)
                for (int i = 1; i < 24; i++)
                    for (int k = 0; k < GetAmountOfColumns(i) - 1; k++)
                    {
                        if(i % 2 == 1)
                        {
                            VertexNode vertexNode = BoardData.Vertices[GetPieceLocationInArray(i, k)];
                            if (vertexNode.PieceType == BoardModel.PieceType.Town && vertexNode.PlayerIndex == Game.PlayerIndicator)
                                BoardPieceButtons[i][k].BorderWidth = Keys.ButtonVisible;
                        }
                    }
        }
    }
}
