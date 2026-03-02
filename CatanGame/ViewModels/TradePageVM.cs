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
        public bool[] CenTradeFourTwoOne => new bool[] { game.PlayerWoodCount >= 4, game.PlayerBrickCount >= 4, game.PlayerSheepCount >= 4, game.PlayerWheatCount >= 4, game.PlayerOreCount >= 4 };
        public bool IsVisibleTradeWithPlayer { get; set; } = false;
        public bool IsVisibleTradeWithBank { get; set; } = false;
        public bool IsVisibleTradeHub => !(IsVisibleTradeWithPlayer || IsVisibleTradeWithBank);
        public bool IsVisibleBackButton => IsVisibleTradeWithPlayer || IsVisibleTradeWithBank;

        public TradePageVM(Game game)
        {
            this.game = game;
            ClosePopupCommand = new Command(ClosePopup);
            GoToTradeWithPlayerCommand = new Command(GoToTradeWithPlayer);
            GoToTradeWithBankCommand = new Command(GoToTradeWithBank);
            BackToTradeHubCommand = new Command(ReturnToTradeHub);
            TradeWithBankCommand = new Command(TradeWithBank);
        }
        private void TradeWithBank(object parameter)
        {
            game.TradeWithBank(parameter);
        }
        private void ClosePopup(object parameter)
        {
            if (parameter is Popup popup)
                popup.Close();
        }
        private void GoToTradeWithPlayer()
        {
            IsVisibleTradeWithPlayer = true;
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
        }
        private void GoToTradeWithBank()
        {
            IsVisibleTradeWithBank = true;
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeWithBank));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
        }
        private void ReturnToTradeHub()
        {
            IsVisibleTradeWithPlayer = false;
            IsVisibleTradeWithBank = false;
            OnPropertyChanged(nameof(IsVisibleBackButton));
            OnPropertyChanged(nameof(IsVisibleTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleTradeWithBank));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
        }
    }
}
