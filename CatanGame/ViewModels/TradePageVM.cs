using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class TradePageVM : ObservableObject
    {
        readonly Game game;
        public ICommand ClosePopupCommand { get; }
        public ICommand GoToTradeWithPlayerCommand { get; }
        public ICommand GoToTradeWithBankCommand { get; }
        public ICommand BackToTradeHubCommand { get; }
        public ICommand TradeWithBankCommand { get; }
        public ICommand PickCardToGetCommand { get; }
        public ICommand ConfirmTradeWithBankCommand { get; }
        public bool[] CenTradeFourToOne => [game.PlayerWoodCount >= 4, game.PlayerBrickCount >= 4, game.PlayerSheepCount >= 4, game.PlayerWheatCount >= 4, game.PlayerOreCount >= 4];
        public bool[] CenTradeThreeToOne => [game.PlayerWoodCount >= 3, game.PlayerBrickCount >= 3, game.PlayerSheepCount >= 3, game.PlayerWheatCount >= 3, game.PlayerOreCount >= 3];
        public bool[] CenTradeTwoToOne => [game.PlayerWoodCount >= 2, game.PlayerBrickCount >= 2, game.PlayerSheepCount >= 2, game.PlayerWheatCount >= 2, game.PlayerOreCount >= 2];
        public bool[] OwnsHarbors => game.OwnsHarbors;
        public bool IsVisiblePickACard { get; set; } = false;
        public bool IsVisibleTradeWithPlayer { get; set; } = false;
        public bool IsVisibleTradeWithBank { get; set; } = false;
        public bool IsVisibleTradeHub { get; set; } = true;
        public bool IsVisibleBackButton => IsVisibleTradeWithPlayer || IsVisibleTradeWithBank || IsVisiblePickACard;

        public TradePageVM(Game game)
        {
            this.game = game;
            ClosePopupCommand = new Command(ClosePopup);
            GoToTradeWithPlayerCommand = new Command(GoToTradeWithPlayer);
            GoToTradeWithBankCommand = new Command(GoToTradeWithBank);
            BackToTradeHubCommand = new Command(ReturnToTradeHub);
            TradeWithBankCommand = new Command(TradeWithBank);
            PickCardToGetCommand = new Command(PickCardToGet);
            ConfirmTradeWithBankCommand = new Command(ConfirmTradeWithBank);
        }

        private void TradeWithBank(object parameter)
        {
            game.TradeWithBank(parameter);
            IsVisibleTradeWithBank = false;
            IsVisiblePickACard = true;
            OnPropertyChanged(nameof(IsVisibleTradeWithBank));
            OnPropertyChanged(nameof(IsVisiblePickACard));
            UpdateCenTradeLists();
        }
        private void PickCardToGet(object parameter)
        {
            game.PickCardToGet(parameter);
        }
        private void ConfirmTradeWithBank()
        {
            game.ConfirmTradeWithBank();
            IsVisibleTradeHub = true;
            IsVisiblePickACard = false;
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
            OnPropertyChanged(nameof(IsVisiblePickACard));
            UpdateCenTradeLists();
        }
        private void ClosePopup(object parameter)
        {
            if (parameter is Popup popup)
                popup.Close();
        }
        private void GoToTradeWithPlayer()
        {
            IsVisibleTradeHub = false;
            IsVisibleTradeWithPlayer = true;
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
        }
        private void GoToTradeWithBank()
        {
            IsVisibleTradeHub = false;
            IsVisibleTradeWithBank = true;
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeWithBank));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
        }
        private void ReturnToTradeHub()
        {
            IsVisibleTradeHub = true;
            IsVisibleTradeWithPlayer = false;
            IsVisibleTradeWithBank = false;
            IsVisiblePickACard = false;
            OnPropertyChanged(nameof(IsVisiblePickACard));
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleTradeWithBank));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
        }
        private void UpdateCenTradeLists()
        {
            OnPropertyChanged(nameof(CenTradeFourToOne));
            OnPropertyChanged(nameof(CenTradeThreeToOne));
            OnPropertyChanged(nameof(CenTradeTwoToOne));
        }
    }
}
