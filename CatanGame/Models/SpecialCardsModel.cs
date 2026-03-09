using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public abstract class SpecialCardsModel
    {
        public GameGrid GameGrid { get; set; } = new();
        public Game Game { get; set; } = new();
        public Board Board { get; set; } = new();

        protected abstract void ShowKnightRobberPlacmentOptions();
        public abstract void UseKnight();
        public abstract void UseRoadBuilding();
        public abstract void UseYearOfPlenty();
        public abstract void UseMonopoly();
        public abstract void GetCardFromPackege();
        public abstract void ReturnCardToPackege();
    }
}
