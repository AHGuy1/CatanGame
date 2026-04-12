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
        public abstract void ConfirmSelectedCards(object parameter);
        public abstract void PickCardsToGet(object parameter);
        public abstract void ConfirmSelectedCard(object parameter);
        public abstract void PickCardToGet(object parameter);
        public abstract void UseKnight();
        public abstract void UseRoadBuilding();
        public abstract void UseYearOfPlenty();
        public abstract void UseMonopoly();
        public abstract void GetCardFromPackege();
        #endregion

        #region PrivateMethods
        protected abstract void UpdateCardPack();
        protected abstract void ShowKnightRobberPlacmentOptions();
        protected abstract void ClosePopUp(object parameter);
        protected abstract void ReturnCardToPackege(string card);
        #endregion
    }
}
