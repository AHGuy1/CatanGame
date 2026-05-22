using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public abstract class SpecialCardsModel
    {
        #region Enums
        public enum RoadBuilding
        {
            Disabled = 0,
            First = 1,
            Second = 2,
        }
        #endregion

        #region Fields
        protected GameGrid? GameGrid { get; set; }
        protected Game? Game { get; set; }
        protected Board? Board { get; set; }
        protected ImageButton? SelectedCard { get; set; }
        #endregion

        #region Properties
        //Index 0 = Knight, Index 1 = University, Index 2 = Road Building, Index 3 = Monopoley, Index 4 = YearOfPlenty, Index 5 = CardBackGroud
        public int[] SpecialCardCounters { get; set; } = new int[5];
        //Index 0 = Wood, Index 1 = Brick, Index 2 = Sheep, Index 3 = Wheat, Index 4 = Ore
        public int[] SelectedGetCounters { get; set; } = new int[5];
        public int TotalSelectedCount => SelectedGetCounters[0] + SelectedGetCounters[1] + SelectedGetCounters[2] + SelectedGetCounters[3] + SelectedGetCounters[4];
        public string[] CardPack { get; set; } = new string[25];
        public RoadBuilding RoadBuildingStuatus { get; set; } = RoadBuilding.Disabled;
        #endregion

        #region PublicMethods
        // Confirms selected Year of Plenty resources.
        public abstract void ConfirmSelectedCards(object parameter);
        // Selects Year of Plenty resources.
        public abstract void PickCardsToGet(object parameter);
        // Confirms the selected Monopoly resource.
        public abstract void ConfirmSelectedCard(object parameter);
        // Selects a Monopoly resource card.
        public abstract void PickCardToGet(object parameter);
        // Uses a knight card.
        public abstract void UseKnight();
        // Uses a Road Building card.
        public abstract void UseRoadBuilding();
        // Uses a Year of Plenty card.
        public abstract void UseYearOfPlenty();
        // Uses a Monopoly card.
        public abstract void UseMonopoly();
        // Draws a development card.
        public abstract void GetCardFromPackege();
        #endregion

        #region PrivateMethods
        // Syncs the development card pack.
        protected abstract void UpdateCardPack();
        // Shows robber options for a knight card.
        protected abstract void ShowKnightRobberPlacmentOptions();
        // Closes the active popup.
        protected abstract void ClosePopUp(object parameter);
        // Returns a used card to the pack.
        protected abstract void ReturnCardToPackege(string card);
        #endregion
    }
}
