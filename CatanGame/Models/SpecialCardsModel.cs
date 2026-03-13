using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public abstract class SpecialCardsModel
    {
        protected GameGrid? GameGrid { get; set; }
        protected Game? Game { get; set; }
        protected Board? Board { get; set; }
        protected ImageButton? SelectedImage { get; set; }

        public int PlayerKnightCount { get; set; }
        public int PlayerUniversityCount { get; set; }
        public int PlayerRoadBuildingCount { get; set; }
        public int PlayerMonopolyCount { get; set; }
        public int PlayerYearOfPlentyCount { get; set; }
        public int SelectedWoodCount { get; set; }
        public int SelectedBrickCount { get; set; }
        public int SelectedSheepCount { get; set; }
        public int SelectedWheatCount { get; set; }
        public int SelectedOreCount { get; set; }
        public int TotalSelectedCount => SelectedWoodCount + SelectedBrickCount + SelectedSheepCount + SelectedWheatCount + SelectedOreCount;
        public string[] CardPack { get; set; } = new string[25];
        public RoadBuilding RoadBuildingStuatus { get; set; } = RoadBuilding.Disabled;

        public enum RoadBuilding
        {
            Disabled = 0,
            First = 1,
            Second = 2,
        }

        protected abstract void UpdateCardPack();
        protected abstract void ShowKnightRobberPlacmentOptions();
        protected abstract void ClosePopUp(object parameter);
        protected abstract void ReturnCardToPackege(string card);

        public abstract void ConfirmSelectedCards(object parameter);
        public abstract void PickCardsToGet(object parameter);
        public abstract void ConfirmSelectedCard(object parameter);
        public abstract void PickCardToGet(object parameter);
        public abstract void UseKnight();
        public abstract void UseRoadBuilding();
        public abstract void UseYearOfPlenty();
        public abstract void UseMonopoly();
        public abstract void GetCardFromPackege();
    }
}
