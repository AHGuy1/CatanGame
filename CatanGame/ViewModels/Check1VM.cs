using CatanGame.ModelsLogic;
using CatanGame.Views;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public class Check1VM
    {
        public SpecialCards s { get; set; }
        public Check1VM() 
        {
            Game game = new();
            GameGrid grid = new(game);
            Board board = new();
            s = new(grid, game, board);
            Command = new Command(UseYearOfPlenty);
        }

        // Opens the special card test popup.
        private void UseYearOfPlenty(object parameter)
        {
            if(parameter is Check1 r)
            {
                MonopolyPage monopolyPage = new(s);
                r.ShowPopup(monopolyPage);
            }
        }
        public ICommand Command { get; }
    }
}
