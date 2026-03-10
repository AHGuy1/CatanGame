using CatanGame.Models;
using CommunityToolkit.Maui.Views;
using CatanGame.Views;

namespace CatanGame.ModelsLogic
{
    public class SpecialCards : SpecialCardsModel
    {
        public SpecialCards(GameGrid gameGrid, Game game, Board board)
        {
            GameGrid =gameGrid;
            Game = game;
            Board = board;
        }
        public SpecialCards() { }

        protected override void ShowKnightRobberPlacmentOptions()
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                for (int i = 0; i < 5; i++)
                    for (int k = 0; k < GameGrid.GetAmountOfColumnsTiles(i + 1); k++)
                        if (Board!.Hexes[GameGrid.GetTileLocationInArray(i + 1, k + 1)].HasRobber)
                        {
                            if (k != GameGrid.GetAmountOfColumnsTiles(i + 1) - 1)
                                GameGrid!.SetVisibleRobberImages(i, k + 1);
                            if (k != 0)
                                GameGrid!.SetVisibleRobberImages(i, k - 1);
                            if (i < 2)
                            {
                                GameGrid!.SetVisibleRobberImages(i + 1, k);
                                GameGrid.SetVisibleRobberImages(i + 1, k + 1);
                            }
                            if (i > 0 && i < 3)
                            {
                                if(k == 0)
                                    GameGrid!.SetVisibleRobberImages(i - 1, k);
                                else if(k == GameGrid.GetAmountOfColumnsTiles(i + 1) - 1)
                                    GameGrid!.SetVisibleRobberImages(i - 1, k - 1);
                                else
                                {
                                    GameGrid!.SetVisibleRobberImages(i - 1, k - 1);
                                    GameGrid.SetVisibleRobberImages(i - 1, k);
                                }
                            }
                            if(i > 2 && i < 5)
                            {
                                GameGrid!.SetVisibleRobberImages(i - 1, k);
                                GameGrid.SetVisibleRobberImages(i - 1, k + 1);
                            }
                            if(i > 1 && i < 4)
                            {
                                if (k == 0)
                                    GameGrid!.SetVisibleRobberImages(i + 1, k);
                                else if (k == GameGrid.GetAmountOfColumnsTiles(i + 1) - 1)
                                    GameGrid!.SetVisibleRobberImages(i + 1, k - 1);
                                else
                                {
                                    GameGrid!.SetVisibleRobberImages(i + 1, k - 1);
                                    GameGrid!.SetVisibleRobberImages(i + 1, k);
                                }
                            }
                        }
            });
        }
        protected override void ClosePopUp(object parameter)
        {
            if (parameter is Popup popup)
                popup.Close();
        }

        public override void PickCardsToGet(object parameter)
        {
            if (parameter is ImageButton button)
            {
                if(TotalSelectedCount < 2)
                {
                    if(button.BorderWidth == 0)
                        button.BorderWidth = 5;
                    if (button.Source.ToString()!.Contains(Strings.WoodImage))
                        SelectedWoodCount++;
                    else if (button.Source.ToString()!.Contains(Strings.BrickImage))
                        SelectedBrickCount++;
                    else if (button.Source.ToString()!.Contains(Strings.SheepImage))
                        SelectedSheepCount++;
                    else if (button.Source.ToString()!.Contains(Strings.WheatImage))
                        SelectedWheatCount++;
                    else if (button.Source.ToString()!.Contains(Strings.OreImage))
                        SelectedOreCount++;
                }
                else if(button.BorderWidth != 0)
                {
                    button.BorderWidth = 0;
                    if (button.Source.ToString()!.Contains(Strings.WoodImage))
                        SelectedWoodCount = 0;
                    else if (button.Source.ToString()!.Contains(Strings.BrickImage))
                        SelectedBrickCount = 0;
                    else if (button.Source.ToString()!.Contains(Strings.SheepImage))
                        SelectedSheepCount = 0;
                    else if (button.Source.ToString()!.Contains(Strings.WheatImage))
                        SelectedWheatCount = 0;
                    else if (button.Source.ToString()!.Contains(Strings.OreImage))
                        SelectedOreCount = 0;
                }
            }
        }
        public override void ConfirmSelectedCards(object parameter)
        {
            Game!.PlayerWoodCount += SelectedWoodCount;
            Game!.PlayerBrickCount += SelectedBrickCount;
            Game!.PlayerSheepCount += SelectedSheepCount;
            Game!.PlayerWheatCount += SelectedWheatCount;
            Game!.PlayerOreCount += SelectedOreCount;
            ClosePopUp(parameter);
        }
        public override void PickCardToGet(object parameter)
        {
            if(parameter is ImageButton image)
            {
                if(SelectedImage != null)
                    SelectedImage!.BorderWidth = 0;
                SelectedImage = image;
                SelectedImage.BorderWidth = 5;
            }
        }
        public override void ConfirmSelectedCard(object parameter)
        {
            if(Game != null)
            {
                if (SelectedImage!.Source.ToString()!.Contains(Strings.WoodImage))
                    Game.MonopolizedCard = Strings.WoodImage;
                else if (SelectedImage.Source.ToString()!.Contains(Strings.BrickImage))
                    Game.MonopolizedCard = Strings.BrickImage;
                else if (SelectedImage.Source.ToString()!.Contains(Strings.SheepImage))
                    Game.MonopolizedCard = Strings.SheepImage;
                else if (SelectedImage.Source.ToString()!.Contains(Strings.WheatImage))
                    Game.MonopolizedCard = Strings.WheatImage;
                else if (SelectedImage.Source.ToString()!.Contains(Strings.OreImage))
                    Game.MonopolizedCard = Strings.OreImage;
                Game.MonoplizingPlayer = Game.PlayerNames[Game.PlayerIndicator];
                Game.PlayersPassed = 1;
                Dictionary<string, object> dict = new()
                {
                    {nameof(Game.PlayersPassed),Game.PlayersPassed },
                    {nameof(Game.MonopolizedCard), Game.MonopolizedCard },
                    {nameof(Game.MonoplizingPlayer), Game.MonoplizingPlayer }
                };
                Game.UpdateFields(dict);
            }
            ClosePopUp(parameter);
        }
        public override void UseKnight()
        {
            ShowKnightRobberPlacmentOptions();
        }
        public override void UseRoadBuilding()
        {
            RoadBuildingStuatus = RoadBuilding.First;
            GameGrid!.ShowBuildOptions(Strings.Road);
        }
        public override void UseYearOfPlenty()
        {
            YearOfPlentyPage yearOfPlentyPage = new(this);
            GameGrid?.CurrentGamePage?.ShowPopup(yearOfPlentyPage);
        }
        public override void UseMonopoly()
        {
            SelectedWoodCount = 0;
            SelectedBrickCount = 0;
            SelectedSheepCount = 0;
            SelectedOreCount = 0;
            SelectedOreCount = 0;
        }
        public override void GetCardFromPackege()
        {
            
        }
        public override void ReturnCardToPackege()
        {
            
        }
    }
}
