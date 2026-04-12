using CatanGame.Models;
using CatanGame.ModelsLogic;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public class YearOfPlentyPageVM : ObservableObject
    {
        #region Fields
        private readonly SpecialCards SpecialCards;
        #endregion

        #region Commands
        public ICommand PickCardToGetCommand { get; }
        public ICommand ConfirmCommand { get; }
        #endregion

        #region Properties
        public string[] SelectedGetCounters => [SpecialCards.SelectedGetCounters[0].ToString(), SpecialCards.SelectedGetCounters[1].ToString(),
            SpecialCards.SelectedGetCounters[2].ToString(), SpecialCards.SelectedGetCounters[3].ToString(), SpecialCards.SelectedGetCounters[4].ToString()];
        #endregion

        #region Constructor
        public YearOfPlentyPageVM(SpecialCards specialCards)
        {
            SpecialCards = specialCards;
            PickCardToGetCommand = new Command(PickCardToGet);
            ConfirmCommand = new Command(ConfirmSelectedCards);
        }
        #endregion

        #region Private Methods
        private void PickCardToGet(object parameter)
        {
            SpecialCards.PickCardsToGet(parameter);
            OnPropertyChanged(nameof(SelectedGetCounters));
        }

        private void ConfirmSelectedCards(object Paramter)
        {
            SpecialCards.ConfirmSelectedCards(Paramter);
        }
        #endregion
    }
}
