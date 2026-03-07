using CatanGame.Models;
using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class TradePageVM : ObservableObject
    {
        private readonly Game game;

        public ICommand ClosePopupCommand { get; }
        public ICommand GoToTradeWithPlayerCommand { get; }
        public ICommand GoToTradeWithBankCommand { get; }
        public ICommand BackToTradeHubCommand { get; }
        public ICommand TradeWithBankCommand { get; }
        public ICommand PickCardToGetCommand { get; }
        public ICommand ConfirmTradeWithBankCommand { get; }
        public ICommand ConfirmTradeWithPlayerCommand { get; }
        public ICommand CaneclTradeCommand { get; }
        public ICommand DeclineTradeCommand { get; }
        public ICommand AcceptTradeCommand { get; }
        public ICommand CounterOfferCommand { get; }
        public bool[] CenTradeFourToOne => [game.PlayerWoodCount >= 4, game.PlayerBrickCount >= 4, game.PlayerSheepCount >= 4, game.PlayerWheatCount >= 4, game.PlayerOreCount >= 4];
        public bool[] CenTradeThreeToOne => [game.PlayerWoodCount >= 3, game.PlayerBrickCount >= 3, game.PlayerSheepCount >= 3, game.PlayerWheatCount >= 3, game.PlayerOreCount >= 3];
        public bool[] CenTradeTwoToOne => [game.PlayerWoodCount >= 2, game.PlayerBrickCount >= 2, game.PlayerSheepCount >= 2, game.PlayerWheatCount >= 2, game.PlayerOreCount >= 2];
        public bool[] OwnsHarbors => game.PlayerOwnedHarbors;
        public bool[] OwnsCards => [game.PlayerWoodCount >= 1, game.PlayerBrickCount >= 1, game.PlayerSheepCount >= 1, game.PlayerWheatCount >= 1, game.PlayerOreCount >= 1];
        public bool[] ReciverGets => [!String.IsNullOrWhiteSpace(WoodGiveAmount) && Convert.ToInt32(WoodGiveAmount) > 0, !String.IsNullOrWhiteSpace(BrickGiveAmount) &&
            Convert.ToInt32(BrickGiveAmount) > 0, !String.IsNullOrWhiteSpace(SheepGiveAmount) && Convert.ToInt32(SheepGiveAmount) > 0, !String.IsNullOrWhiteSpace(WheatGiveAmount)
            && Convert.ToInt32(WheatGiveAmount) > 0, !String.IsNullOrWhiteSpace(OreGiveAmount) && Convert.ToInt32(OreGiveAmount) > 0];
        public bool[] ReciverGives => [!String.IsNullOrWhiteSpace(WoodGetAmount) && Convert.ToInt32(WoodGetAmount) > 0, !String.IsNullOrWhiteSpace(BrickGetAmount) &&
            Convert.ToInt32(BrickGetAmount) > 0, !String.IsNullOrWhiteSpace(SheepGetAmount) && Convert.ToInt32(SheepGetAmount) > 0, !String.IsNullOrWhiteSpace(WheatGetAmount)
            && Convert.ToInt32(WheatGetAmount) > 0, !String.IsNullOrWhiteSpace(OreGetAmount) && Convert.ToInt32(OreGetAmount) > 0];
        public string[] PlayerNames { get; set; }
        public string[] PlayersInTrade => game.PlayersInTrade;
        public bool IsVisiblePickACard { get; set; } = false;
        public bool IsVisibleTradeWithPlayer { get; set; } = false;
        public bool IsVisibleReciveTradeWithPlayer { get; set; }
        public bool IsVisibleTradeWithBank { get; set; } = false;
        public bool IsVisibleTradeHub { get; set; } = true;
        public bool IsVisibleBackButton => IsVisibleTradeWithPlayer || IsVisibleTradeWithBank || IsVisiblePickACard;
        public string SelectedPlayerName
        {
            get => game.SelectedPlayerName;
            set
            {
                game.SelectedPlayerName = value;
                OnPropertyChanged(nameof(SelectedPlayerName));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WoodGiveAmount
        {
            get => game.WoodGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerWoodCount)
                    game.WoodGiveAmount = game.PlayerWoodCount.ToString();
                else
                    game.WoodGiveAmount = value;
                OnPropertyChanged(nameof(WoodGiveAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string BrickGiveAmount
        {
            get => game.BrickGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerBrickCount)
                    game.BrickGiveAmount = game.PlayerBrickCount.ToString();
                else
                    game.BrickGiveAmount = value;
                OnPropertyChanged(nameof(BrickGiveAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string SheepGiveAmount
        {
            get => game.SheepGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerSheepCount)
                    game.SheepGiveAmount = game.PlayerSheepCount.ToString();
                else
                    game.SheepGiveAmount = value;
                OnPropertyChanged(nameof(SheepGiveAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WheatGiveAmount
        {
            get => game.WheatGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerWheatCount)
                    game.WheatGiveAmount = game.PlayerWheatCount.ToString();
                else
                    game.WheatGiveAmount = value;
                OnPropertyChanged(nameof(WheatGiveAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string OreGiveAmount
        {
            get => game.OreGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerOreCount)
                    game.OreGiveAmount = game.PlayerOreCount.ToString();
                else
                    game.OreGiveAmount = value;
                OnPropertyChanged(nameof(OreGiveAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WoodGetAmount
        {
            get => game.WoodGetAmount;
            set
            {
                if(!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25)
                    game.WoodGetAmount = 25.ToString();
                else
                    game.WoodGetAmount = value;
                OnPropertyChanged(nameof(WoodGetAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string BrickGetAmount
        {
                get => game.BrickGetAmount;
                set
                {
                    if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25)
                        game.BrickGetAmount = 25.ToString();
                    else
                        game.BrickGetAmount = value;
                    OnPropertyChanged(nameof(BrickGetAmount));
                    (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
                }
        }
        public string SheepGetAmount
        {
            get => game.SheepGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25)
                    game.SheepGetAmount = 25.ToString();
                else
                    game.SheepGetAmount = value;
                OnPropertyChanged(nameof(SheepGetAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WheatGetAmount
        {
            get => game.WheatGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25)
                    game.WheatGetAmount = 25.ToString();
                else
                    game.WheatGetAmount = value;
                OnPropertyChanged(nameof(WheatGetAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string OreGetAmount
        {
            get => game.OreGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25)
                    game.OreGetAmount = 25.ToString();
                else
                    game.OreGetAmount = value;
                OnPropertyChanged(nameof(OreGetAmount));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }

        public TradePageVM(Game game)
        {
            this.game = game;
            IsVisibleReciveTradeWithPlayer = game.TradeInProgress && PlayersInTrade[1] == game.PlayerNames[game.PlayerIndicator];
            IsVisibleTradeHub = !IsVisibleReciveTradeWithPlayer;
            ClosePopupCommand = new Command(ClosePopup);
            GoToTradeWithPlayerCommand = new Command(GoToTradeWithPlayer);
            GoToTradeWithBankCommand = new Command(GoToTradeWithBank);
            BackToTradeHubCommand = new Command(ReturnToTradeHub);
            TradeWithBankCommand = new Command(TradeWithBank);
            PickCardToGetCommand = new Command(PickCardToGet);
            ConfirmTradeWithBankCommand = new Command(ConfirmTradeWithBank);
            DeclineTradeCommand = new Command(DeclineTrade);
            AcceptTradeCommand = new Command(AcceptTrade);
            CounterOfferCommand = new Command(CounterOffer);
            CaneclTradeCommand = new Command(CancelTradeRequest, CenCancelTradeRequest);
            ConfirmTradeWithPlayerCommand = new Command(ConfirmTradeWithPlayer, CenTradeWithPlayer);
            PlayerNames = new string[game.PlayerNames.Length - 1];
            for (int i = 0; i < PlayerNames.Length; i++)
            {
                if (i != game.PlayerIndicator)
                    PlayerNames[i] = game.PlayerNames[i];
            }
        }

        private void CounterOffer()
        {
            game.CounterOffer();
            IsVisibleTradeWithPlayer = true;
            IsVisibleReciveTradeWithPlayer = false;
            OnPropertyChanged(nameof(IsVisibleTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleReciveTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleBackButton));
        }
        private void AcceptTrade(object parameter)
        {
            game.AcceptTrade();
            ClosePopup(parameter);
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeAccepted, ToastDuration.Long, 20).Show();
            });
        }
        private void DeclineTrade(object parameter)
        {
            game.DeclineTrade();
            ClosePopup(parameter);
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeDeclined, ToastDuration.Long, 20).Show();
            });
        }
        private bool CenCancelTradeRequest()
        {
            return game.TradeInProgress && PlayersInTrade[0] == game.PlayerNames[game.PlayerIndicator];
        }
        private void CancelTradeRequest()
        {
            game.CancelTradeRequest();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeCanceled, ToastDuration.Long, 20).Show();
            });
        }
        private bool CenTradeWithPlayer(object parameter)
        {
            return game.CenTradeWithPlayer(); 
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
        private void ConfirmTradeWithPlayer(object parameter)
        {
            game.ConfirmTradeWithPlayer();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeSent, ToastDuration.Long, 18).Show();
            });
            ClosePopup(parameter);
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
