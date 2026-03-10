using CatanGame.ModelsLogic;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public class MonopolyPageVM
    {
        private readonly SpecialCards SpecialCards;

        public ICommand PickCardToGetCommand { get; }
        public ICommand ConfirmCommand { get; }

        public MonopolyPageVM(SpecialCards specialCards)
        {
            SpecialCards = specialCards;
            PickCardToGetCommand = new Command(PickCardToGet);
            ConfirmCommand = new Command(ConfirmSelectedCards);
        }

        private void PickCardToGet(object parameter)
        {
            SpecialCards.PickCardToGet(parameter);
        }
        private void ConfirmSelectedCards(object Paramter)
        {
            SpecialCards.ConfirmSelectedCard(Paramter);
        }
    }
}
