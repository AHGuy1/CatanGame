using CatanGame.ModelsLogic;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public class MonopolyPageVM
    {
        #region Fields
        private readonly SpecialCards SpecialCards;
        #endregion

        #region Properties
        public ICommand PickCardToGetCommand { get; }
        public ICommand ConfirmCommand { get; }
        #endregion

        #region Constructor
        public MonopolyPageVM(SpecialCards specialCards)
        {
            SpecialCards = specialCards;
            PickCardToGetCommand = new Command(PickCardToGet);
            ConfirmCommand = new Command(ConfirmSelectedCard);
        }
        #endregion

        #region Private Methods
        private void PickCardToGet(object parameter)
        {
            SpecialCards.PickCardToGet(parameter);
        }

        private void ConfirmSelectedCard(object parameter)
        {
            SpecialCards.ConfirmSelectedCard(parameter);
        }
        #endregion
    }
}
