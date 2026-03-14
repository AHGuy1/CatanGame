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
        #region Fields
        private readonly Game game;
        #endregion

        #region Commands
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
        #endregion

        #region Properties
        public bool[] CenTradeFourToOne => [game.PlayerWoodCount >= 4, game.PlayerBrickCount >= 4, game.PlayerSheepCount >= 4, game.PlayerWheatCount >= 4, game.PlayerOreCount >= 4];
        public bool[] CenTradeThreeToOne => [game.PlayerWoodCount >= 3, game.PlayerBrickCount >= 3, game.PlayerSheepCount >= 3, game.PlayerWheatCount >= 3, game.PlayerOreCount >= 3];
        public bool[] CenTradeTwoToOne => [game.PlayerWoodCount >= 2, game.PlayerBrickCount >= 2, game.PlayerSheepCount >= 2, game.PlayerWheatCount >= 2, game.PlayerOreCount >= 2];
        public bool[] OwnsHarbors => game.PlayerOwnedHarbors;
        public bool[] OwnsCards => [game.PlayerWoodCount >= 1, game.PlayerBrickCount >= 1, game.PlayerSheepCount >= 1, game.PlayerWheatCount >= 1, game.PlayerOreCount >= 1];
        public bool[] ReciverGets => 
            [
                !string.IsNullOrWhiteSpace(WoodGiveAmount) && Convert.ToInt32(WoodGiveAmount) > 0,
                !string.IsNullOrWhiteSpace(BrickGiveAmount) && Convert.ToInt32(BrickGiveAmount) > 0,
                !string.IsNullOrWhiteSpace(SheepGiveAmount) && Convert.ToInt32(SheepGiveAmount) > 0,
                !string.IsNullOrWhiteSpace(WheatGiveAmount) && Convert.ToInt32(WheatGiveAmount) > 0,
                !string.IsNullOrWhiteSpace(OreGiveAmount) && Convert.ToInt32(OreGiveAmount) > 0
            ];
        public bool[] ReciverGives => 
            [
                !string.IsNullOrWhiteSpace(WoodGetAmount) && Convert.ToInt32(WoodGetAmount) > 0,
                !string.IsNullOrWhiteSpace(BrickGetAmount) && Convert.ToInt32(BrickGetAmount) > 0,
                !string.IsNullOrWhiteSpace(SheepGetAmount) && Convert.ToInt32(SheepGetAmount) > 0,
                !string.IsNullOrWhiteSpace(WheatGetAmount) && Convert.ToInt32(WheatGetAmount) > 0,
                !string.IsNullOrWhiteSpace(OreGetAmount) && Convert.ToInt32(OreGetAmount) > 0
            ];
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
            get => game.WoodTradeGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerWoodCount && IsVisibleTradeWithPlayer)
                    game.WoodTradeGiveAmount = game.PlayerWoodCount.ToString();
                else
                    game.WoodTradeGiveAmount = value;
                OnPropertyChanged(nameof(WoodGiveAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string BrickGiveAmount
        {
            get => game.BrickTradeGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerBrickCount && IsVisibleTradeWithPlayer)
                    game.BrickTradeGiveAmount = game.PlayerBrickCount.ToString();
                else
                    game.BrickTradeGiveAmount = value;
                OnPropertyChanged(nameof(BrickGiveAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string SheepGiveAmount
        {
            get => game.SheepTradeGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerSheepCount && IsVisibleTradeWithPlayer)
                    game.SheepTradeGiveAmount = game.PlayerSheepCount.ToString();
                else
                    game.SheepTradeGiveAmount = value;
                OnPropertyChanged(nameof(SheepGiveAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WheatGiveAmount
        {
            get => game.WheatTradeGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerWheatCount && IsVisibleTradeWithPlayer)
                    game.WheatTradeGiveAmount = game.PlayerWheatCount.ToString();
                else
                    game.WheatTradeGiveAmount = value;
                OnPropertyChanged(nameof(WheatGiveAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string OreGiveAmount
        {
            get => game.OreTradeGiveAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > game.PlayerOreCount && IsVisibleTradeWithPlayer)
                    game.OreTradeGiveAmount = game.PlayerOreCount.ToString();
                else
                    game.OreTradeGiveAmount = value;
                OnPropertyChanged(nameof(OreGiveAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WoodGetAmount
        {
            get => game.WoodTradeGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25 && IsVisibleTradeWithPlayer)
                    game.WoodTradeGetAmount = 25.ToString();
                else
                    game.WoodTradeGetAmount = value;
                OnPropertyChanged(nameof(WoodGetAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string BrickGetAmount
        {
            get => game.BrickTradeGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25 && IsVisibleTradeWithPlayer)
                    game.BrickTradeGetAmount = 25.ToString();
                else
                    game.BrickTradeGetAmount = value;
                OnPropertyChanged(nameof(BrickGetAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string SheepGetAmount
        {
            get => game.SheepTradeGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25 && IsVisibleTradeWithPlayer)
                    game.SheepTradeGetAmount = 25.ToString();
                else
                    game.SheepTradeGetAmount = value;
                OnPropertyChanged(nameof(SheepGetAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string WheatGetAmount
        {
            get => game.WheatTradeGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25 && IsVisibleTradeWithPlayer)
                    game.WheatTradeGetAmount = 25.ToString();
                else
                    game.WheatTradeGetAmount = value;
                OnPropertyChanged(nameof(WheatGetAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        public string OreGetAmount
        {
            get => game.OreTradeGetAmount;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 25 && IsVisibleTradeWithPlayer)
                    game.OreTradeGetAmount = 25.ToString();
                else
                    game.OreTradeGetAmount = value;
                OnPropertyChanged(nameof(OreGetAmount));
                OnPropertyChanged(nameof(ReciverGets));
                OnPropertyChanged(nameof(ReciverGives));
                (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
            }
        }
        #endregion

        #region Constructor
        public TradePageVM(Game game)
        {
            this.game = game;
            IsVisibleReciveTradeWithPlayer = game.TradeInProgress && PlayersInTrade[1] == game.PlayerNames[game.PlayerIndicator];
            IsVisibleTradeHub = !IsVisibleReciveTradeWithPlayer;
            OnPropertyChanged(nameof(IsVisibleReciveTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleTradeHub));
            ClosePopupCommand = new Command(ClosePopup);
            GoToTradeWithPlayerCommand = new Command(GoToTradeWithPlayer, CenTrade);
            GoToTradeWithBankCommand = new Command(GoToTradeWithBank, CenTrade);
            BackToTradeHubCommand = new Command(ReturnToTradeHub);
            TradeWithBankCommand = new Command(TradeWithBank);
            PickCardToGetCommand = new Command(PickCardToGet);
            ConfirmTradeWithBankCommand = new Command(ConfirmTradeWithBank);
            DeclineTradeCommand = new Command(DeclineTrade);
            AcceptTradeCommand = new Command(AcceptTrade, CenAcceptTrade);
            CounterOfferCommand = new Command(CounterOffer);
            CaneclTradeCommand = new Command(CancelTradeRequest, CenCancelTradeRequest);
            ConfirmTradeWithPlayerCommand = new Command(ConfirmTradeWithPlayer, CenTradeWithPlayer);
            PlayerNames = game.GetPlayersToTradeWith();
        }
        #endregion

        #region Private Methods
        private void RefreshTradeParameters()
        {
            OnPropertyChanged(nameof(SelectedPlayerName));
            OnPropertyChanged(nameof(PlayersInTrade));
            OnPropertyChanged(nameof(SelectedPlayerName));
            OnPropertyChanged(nameof(WoodGiveAmount));
            OnPropertyChanged(nameof(BrickGiveAmount));
            OnPropertyChanged(nameof(SheepGiveAmount));
            OnPropertyChanged(nameof(WheatGiveAmount));
            OnPropertyChanged(nameof(OreGiveAmount));
            OnPropertyChanged(nameof(WoodGetAmount));
            OnPropertyChanged(nameof(BrickGetAmount));
            OnPropertyChanged(nameof(SheepGetAmount));
            OnPropertyChanged(nameof(WheatGetAmount));
            OnPropertyChanged(nameof(OreGetAmount));
            OnPropertyChanged(nameof(ReciverGets));
            OnPropertyChanged(nameof(ReciverGives));
        }

        private void CounterOffer()
        {
            game.CounterOffer();
            PlayerNames = [SelectedPlayerName];
            IsVisibleTradeWithPlayer = true;
            IsVisibleReciveTradeWithPlayer = false;
            OnPropertyChanged(nameof(IsVisibleTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleReciveTradeWithPlayer));
            OnPropertyChanged(nameof(IsVisibleBackButton));
            RefreshTradeParameters();
            (ConfirmTradeWithPlayerCommand as Command)?.ChangeCanExecute();
        }

        private void AcceptTrade()
        {
            game.AcceptTrade();
            ClosePopup();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeAccepted, ToastDuration.Long, 20).Show();
            });
            OnPropertyChanged(nameof(IsVisibleTradeHub));
            OnPropertyChanged(nameof(IsVisibleReciveTradeWithPlayer));
        }

        private void DeclineTrade()
        {
            game.DeclineTrade();
            ClosePopup();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeDeclined, ToastDuration.Long, 20).Show();
            });
            OnPropertyChanged(nameof(IsVisibleTradeHub));
            OnPropertyChanged(nameof(IsVisibleReciveTradeWithPlayer));
        }

        private void CancelTradeRequest()
        {
            game.CancelTradeRequest();
            (CaneclTradeCommand as Command)?.ChangeCanExecute();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeCanceled, ToastDuration.Long, 20).Show();
            });
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

        private void ConfirmTradeWithPlayer()
        {
            game.ConfirmTradeWithPlayer();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.TradeSent, ToastDuration.Long, 18).Show();
            });
            ClosePopup();
        }

        private void ClosePopup()
        {
            game.CloseTrade();
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

        private bool CenCancelTradeRequest()
        {
            return game.TradeInProgress && PlayersInTrade[0] == game.PlayerNames[game.PlayerIndicator];
        }

        private bool CenAcceptTrade()
        {
            return game.CenAcceptTrade();
        }

        private bool CenTradeWithPlayer()
        {
            return game.CenTradeWithPlayer();
        }

        private bool CenTrade()
        {
            return !game.TradeInProgress;
        }
        #endregion
    }
}
