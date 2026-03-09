using CatanGame.Models;
using static Android.InputMethodServices.Keyboard;

namespace CatanGame.ModelsLogic
{
    public class SpecialCards : SpecialCardsModel
    {
        public SpecialCards(GameGrid GameGrid)
        {
            this.GameGrid = GameGrid;
            Game = GameGrid.Game;
            Board = Game.GameBoard;
        }
        public override void UseKnight()
        {
            ShowKnightRobberPlacmentOptions();
        }

        protected override void ShowKnightRobberPlacmentOptions()
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                for (int i = 0; i < 5; i++)
                    for (int k = 0; k < GameGrid.GetAmountOfColumnsTiles(i + 1); k++)
                        if (Board.Hexes[GameGrid.GetTileLocationInArray(i + 1, k + 1)].HasRobber)
                        {
                            if (k != GameGrid.GetAmountOfColumnsTiles(i + 1) - 1)
                                GameGrid.SetVisibleRobberImages(i, k + 1);
                            if (k != 0)
                                GameGrid.SetVisibleRobberImages(i, k - 1);
                            if (i < 2)
                            {
                                GameGrid.SetVisibleRobberImages(i + 1, k);
                                GameGrid.SetVisibleRobberImages(i + 1, k + 1);
                            }
                            if (i > 0 && i < 3)
                            {
                                if(k == 0)
                                    GameGrid.SetVisibleRobberImages(i - 1, k);

                            }
                        }
            });
        }
    }
}
