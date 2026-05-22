using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Views;

namespace CatanGame.ModelsLogic
{
    public class SpecialCards : SpecialCardsModel
    {
        #region Constructor
        public SpecialCards(GameGrid gameGrid, Game game, Board board)
        {
            GameGrid = gameGrid;
            Game = game;
            Board = board;
            Random random = new();
            if (Game.PlayerIndicator == 0)
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

        public SpecialCards()
        {
        }
        #endregion

        #region Private Methods
        // Syncs the development card pack to the game.
        protected override void UpdateCardPack()
        {
            if (Game != null)
            {
                Game.SpecialCards = CardPack;
                Dictionary<string, object> dict = new()
                {
                    { nameof(Game.SpecialCards), Game.SpecialCards }
                };
                Game.UpdateFields(dict);
            }
        }

        // Shows robber moves allowed by a knight card.
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
                                if (k == 0)
                                    GameGrid!.SetVisibleRobberImages(i - 1, k);
                                else if (k == GameGrid.GetAmountOfColumnsTiles(i + 1) - 1)
                                    GameGrid!.SetVisibleRobberImages(i - 1, k - 1);
                                else
                                {
                                    GameGrid!.SetVisibleRobberImages(i - 1, k - 1);
                                    GameGrid.SetVisibleRobberImages(i - 1, k);
                                }
                            }
                            if (i > 2 && i < 5)
                            {
                                GameGrid!.SetVisibleRobberImages(i - 1, k);
                                GameGrid.SetVisibleRobberImages(i - 1, k + 1);
                            }
                            if (i > 1 && i < 4)
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

        // Closes the popup passed as a command parameter.
        protected override void ClosePopUp(object parameter)
        {
            if (parameter is Popup popup)
                popup.Close();
        }

        // Returns a used card to the development card pack.
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
        #endregion

        #region Public Methods
        // Selects resources for the Year of Plenty card.
        public override void PickCardsToGet(object parameter)
        {
            if (parameter is ImageButton button)
            {
                if (TotalSelectedCount < 2)
                {
                    if (button.BorderWidth == 0)
                        button.BorderWidth = 5;
                    if (button.Source.ToString()!.Contains(Strings.WoodImage))
                        SelectedGetCounters[0]++;
                    else if (button.Source.ToString()!.Contains(Strings.BrickImage))
                        SelectedGetCounters[1]++;
                    else if (button.Source.ToString()!.Contains(Strings.SheepImage))
                        SelectedGetCounters[2]++;
                    else if (button.Source.ToString()!.Contains(Strings.WheatImage))
                        SelectedGetCounters[3]++;
                    else if (button.Source.ToString()!.Contains(Strings.OreImage))
                        SelectedGetCounters[4]++;
                }
                else if (button.BorderWidth != 0)
                {
                    button.BorderWidth = 0;
                    if (button.Source.ToString()!.Contains(Strings.WoodImage))
                        SelectedGetCounters[0] = 0;
                    else if (button.Source.ToString()!.Contains(Strings.BrickImage))
                        SelectedGetCounters[1] = 0;
                    else if (button.Source.ToString()!.Contains(Strings.SheepImage))
                        SelectedGetCounters[2] = 0;
                    else if (button.Source.ToString()!.Contains(Strings.WheatImage))
                        SelectedGetCounters[3] = 0;
                    else if (button.Source.ToString()!.Contains(Strings.OreImage))
                        SelectedGetCounters[4] = 0;
                }
            }
        }

        // Adds selected Year of Plenty resources to the player.
        public override void ConfirmSelectedCards(object parameter)
        {
            if (Game != null && GameGrid != null)
            {
                Game.PlayerWoodCount += SelectedGetCounters[0];
                Game.PlayerBrickCount += SelectedGetCounters[1];
                Game.PlayerSheepCount += SelectedGetCounters[2];
                Game.PlayerWheatCount += SelectedGetCounters[3];
                Game.PlayerOreCount += SelectedGetCounters[4];
                GameGrid.UpdateResourceCounters();
            }
            ClosePopUp(parameter);
        }

        // Selects a resource for the Monopoly card.
        public override void PickCardToGet(object parameter)
        {
            if (parameter is ImageButton image)
            {
                if (SelectedCard != null)
                    SelectedCard!.BorderWidth = 0;
                SelectedCard = image;
                SelectedCard.BorderWidth = 5;
            }
        }

        // Starts the Monopoly card effect for the selected resource.
        public override void ConfirmSelectedCard(object parameter)
        {
            if (Game != null)
            {
                if (SelectedCard!.Source.ToString()!.Contains(Strings.WoodImage))
                    Game.MonopolizedCard = Strings.WoodImage;
                else if (SelectedCard.Source.ToString()!.Contains(Strings.BrickImage))
                    Game.MonopolizedCard = Strings.BrickImage;
                else if (SelectedCard.Source.ToString()!.Contains(Strings.SheepImage))
                    Game.MonopolizedCard = Strings.SheepImage;
                else if (SelectedCard.Source.ToString()!.Contains(Strings.WheatImage))
                    Game.MonopolizedCard = Strings.WheatImage;
                else if (SelectedCard.Source.ToString()!.Contains(Strings.OreImage))
                    Game.MonopolizedCard = Strings.OreImage;
                Game.MonoplizingPlayer = Game.PlayerNames[Game.PlayerIndicator];
                Game.PlayersPassed = 1;
                Dictionary<string, object> dict = new()
                    {
                        { nameof(Game.PlayersPassed), Game.PlayersPassed },
                        { nameof(Game.MonopolizedCard), Game.MonopolizedCard },
                        { nameof(Game.MonoplizingPlayer), Game.MonoplizingPlayer }
                    };
                Game.UpdateFields(dict);
            }
            ClosePopUp(parameter);
        }

        // Uses a knight card and enables robber movement.
        public override void UseKnight()
        {
            ShowKnightRobberPlacmentOptions();
            SpecialCardCounters[0]--;
        }

        // Uses a Road Building card and starts road placement.
        public override void UseRoadBuilding()
        {
            RoadBuildingStuatus = RoadBuilding.First;
            GameGrid!.ShowBuildOptions(Strings.Road);
            ReturnCardToPackege(Strings.RoadBuildingImage);
            SpecialCardCounters[2]--;
        }

        // Uses a Year of Plenty card and opens its popup.
        public override void UseYearOfPlenty()
        {
            YearOfPlentyPage yearOfPlentyPage = new(this);
            GameGrid?.CurrentGamePage?.ShowPopup(yearOfPlentyPage);
            ReturnCardToPackege(Strings.YearOfPlentyImage);
            SpecialCardCounters[4]--;
        }

        // Uses a Monopoly card and opens its popup.
        public override void UseMonopoly()
        {
            MonopolyPage monopolyPage = new(this);
            GameGrid?.CurrentGamePage?.ShowPopup(monopolyPage);
            ReturnCardToPackege(Strings.MonopolyImage);
            SpecialCardCounters[3]--;
        }

        // Draws the top development card into the player's hand.
        public override void GetCardFromPackege()
        {
            if (CardPack[0] == Strings.KnightImage)
                SpecialCardCounters[0]++;
            else if (CardPack[0] == Strings.UniversityImage)
            {
                SpecialCardCounters[1]++;
                Game!.PlayerVictoryPointCardsCount++;
            }
            else if (CardPack[0] == Strings.MonopolyImage)
                SpecialCardCounters[3]++;
            else if (CardPack[0] == Strings.RoadBuildingImage)
                SpecialCardCounters[2]++;
            else if (CardPack[0] == Strings.YearOfPlentyImage)
                SpecialCardCounters[4]++;
            CardPack[0] = string.Empty;
            for (int i = 0; i < CardPack.Length - 1 && !String.IsNullOrWhiteSpace(CardPack[i + 1]); i++)
                (CardPack[i], CardPack[i + 1]) = (CardPack[i + 1], CardPack[i]);
            UpdateCardPack();
        }
        #endregion
    }
}
