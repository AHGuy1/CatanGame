using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Views;

namespace CatanGame.ModelsLogic
{
    public class SpecialCards : SpecialCardsModel
    {
        public SpecialCards(GameGrid gameGrid, Game game, Board board)
        {
            GameGrid = gameGrid;
            Game = game;
            Board = board;
            Random random = new();
            if(Game.PlayerIndicator == 0)
            {
                if (Game.PlayerCount < 5)
                    CardPack =
                    [
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage,
                        Strings.UniversityImage, Strings.UniversityImage, Strings.UniversityImage,
                        Strings.UniversityImage, Strings.UniversityImage,
                        Strings.MonopolyImage, Strings.RoadBuildingImage, Strings.YearOfPlentyImage,
                        Strings.MonopolyImage, Strings.RoadBuildingImage, Strings.YearOfPlentyImage
                    ];
                else
                    CardPack =
                    [
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.KnightImage, Strings.KnightImage, Strings.KnightImage, Strings.KnightImage,
                        Strings.UniversityImage, Strings.UniversityImage, Strings.UniversityImage,
                        Strings.UniversityImage, Strings.UniversityImage,
                        Strings.MonopolyImage, Strings.RoadBuildingImage, Strings.YearOfPlentyImage,
                        Strings.MonopolyImage, Strings.RoadBuildingImage, Strings.YearOfPlentyImage,
                        Strings.MonopolyImage, Strings.RoadBuildingImage, Strings.YearOfPlentyImage
                    ];
                for (int i = CardPack.Length - 1; i > 0; i--)
                {
                    int location = random.Next(i + 1);
                    (CardPack[i], CardPack[location]) = (CardPack[location], CardPack[i]);
                }
                UpdateCardPack();
            }
        }
        public SpecialCards() { }

        protected override void UpdateCardPack()
        {
            if(Game != null)
            {
                Game.SpecialCards = CardPack;
                Dictionary<string, object> dict = new()
                {
                    {nameof(Game.SpecialCards), Game.SpecialCards}
                };
                Game.UpdateFields(dict);
            }
        }
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
        protected override void ReturnCardToPackege(string card)
        {
            bool found = false;
            for (int i = 0; i < CardPack.Length && !found; i++)
            {
                if (String.IsNullOrWhiteSpace(CardPack[i + 1]))
                {
                    CardPack[i] = card;
                    found = true;
                }
            }
            UpdateCardPack();
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
            if(Game != null && GameGrid != null)
            {
                Game.PlayerWoodCount += SelectedWoodCount;
                Game.PlayerBrickCount += SelectedBrickCount;
                Game.PlayerSheepCount += SelectedSheepCount;
                Game.PlayerWheatCount += SelectedWheatCount;
                Game.PlayerOreCount += SelectedOreCount;
                GameGrid.UpdateResourceCounters();
            }
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
            PlayerKnightCount--;
        }
        public override void UseRoadBuilding()
        {
            RoadBuildingStuatus = RoadBuilding.First;
            GameGrid!.ShowBuildOptions(Strings.Road);
            ReturnCardToPackege(Strings.RoadBuildingImage);
            PlayerRoadBuildingCount--;
        }
        public override void UseYearOfPlenty()
        {
            YearOfPlentyPage yearOfPlentyPage = new(this);
            GameGrid?.CurrentGamePage?.ShowPopup(yearOfPlentyPage);
            ReturnCardToPackege(Strings.YearOfPlentyImage);
            PlayerYearOfPlentyCount--;
        }
        public override void UseMonopoly()
        {
            SelectedWoodCount = 0;
            SelectedBrickCount = 0;
            SelectedSheepCount = 0;
            SelectedOreCount = 0;
            SelectedOreCount = 0;
            MonopolyPage monopolyPage = new(this);
            GameGrid?.CurrentGamePage?.ShowPopup(monopolyPage);
            ReturnCardToPackege(Strings.MonopolyImage);
            PlayerMonopolyCount--;
        }
        public override void GetCardFromPackege()
        {
            if (CardPack[0] == Strings.KnightImage)
                PlayerKnightCount++;
            else if (CardPack[0] == Strings.UniversityImage)
            {
                PlayerUniversityCount++;
                Game!.PlayerVictoryPointCardsCount++;
            }
            else if (CardPack[0] == Strings.MonopolyImage)
                PlayerMonopolyCount++;
            else if (CardPack[0] == Strings.RoadBuildingImage)
                PlayerRoadBuildingCount++;
            else if (CardPack[0] == Strings.YearOfPlentyImage)
                PlayerYearOfPlentyCount++;
            CardPack[0] = string.Empty;
            for (int i = 0; i < CardPack.Length - 1 && !String.IsNullOrWhiteSpace(CardPack[i + 1]); i++)
                (CardPack[i], CardPack[i + 1]) = (CardPack[i + 1], CardPack[i]);
            UpdateCardPack();
        }
    }
}
  