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
        public string SelectedWoodCount => SpecialCards.SelectedWoodCount.ToString();
        public string SelectedBrickCount => SpecialCards.SelectedBrickCount.ToString();
        public string SelectedSheepCount => SpecialCards.SelectedSheepCount.ToString();
        public string SelectedWheatCount => SpecialCards.SelectedWheatCount.ToString();
        public string SelectedOreCount => SpecialCards.SelectedOreCount.ToString();
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
            OnPropertyChanged(nameof(SelectedWoodCount));
            OnPropertyChanged(nameof(SelectedBrickCount));
            OnPropertyChanged(nameof(SelectedSheepCount));
            OnPropertyChanged(nameof(SelectedWheatCount));
            OnPropertyChanged(nameof(SelectedOreCount));
        }

        private void ConfirmSelectedCards(object Paramter)
        {
            SpecialCards.ConfirmSelectedCards(Paramter);
        }
        #endregion
    }
}
