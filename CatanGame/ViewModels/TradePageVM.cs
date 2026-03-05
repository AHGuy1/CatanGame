using CatanGame.Models;
using CatanGame.ModelsLogic;
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
        public ICommand ConfirmTradeWithPlayerCommand { get; }
        public bool[] CenTradeFourToOne => [game.PlayerWoodCount >= 4, game.PlayerBrickCount >= 4, game.PlayerSheepCount >= 4, game.PlayerWheatCount >= 4, game.PlayerOreCount >= 4];
        public bool[] CenTradeThreeToOne => [game.PlayerWoodCount >= 3, game.PlayerBrickCount >= 3, game.PlayerSheepCount >= 3, game.PlayerWheatCount >= 3, game.PlayerOreCount >= 3];
        public bool[] CenTradeTwoToOne => [game.PlayerWoodCount >= 2, game.PlayerBrickCount >= 2, game.PlayerSheepCount >= 2, game.PlayerWheatCount >= 2, game.PlayerOreCount >= 2];
        public bool[] OwnsHarbors => game.PlayerOwnedHarbors;
        public bool[] OwnsCards => [game.PlayerWoodCount >= 1, game.PlayerBrickCount >= 1, game.PlayerSheepCount >= 1, game.PlayerWheatCount >= 1, game.PlayerOreCount >= 1];
        public string[] PlayerNames => ["1","2","3","4","5"];
        public string SelectedPlayerName { get; set; } = string.Empty;
        public bool IsVisiblePickACard { get; set; } = false;
        public bool IsVisibleTradeWithPlayer { get; set; } = false;
        public bool IsVisibleTradeWithBank { get; set; } = false;
        public bool IsVisibleTradeHub { get; set; } = true;
        public bool IsVisibleBackButton => IsVisibleTradeWithPlayer || IsVisibleTradeWithBank || IsVisiblePickACard;
        public string WoodGiveAmount
        {
            get => game.WoodGiveAmount;
            set
            {
                if (Convert.ToInt32(value) > game.PlayerWoodCount)
                    game.WoodGiveAmount = game.PlayerWoodCount.ToString();
                else
                    game.WoodGiveAmount = value;
                OnPropertyChanged(nameof(WoodGiveAmount));
            }
        }
        public string BrickGiveAmount
        {
            get => game.BrickGiveAmount;
            set
            {
                if (Convert.ToInt32(value) > game.PlayerBrickCount)
                    game.BrickGiveAmount = game.PlayerBrickCount.ToString();
                else
                    game.BrickGiveAmount = value;
                OnPropertyChanged(nameof(BrickGiveAmount));
            }
        }
        public string SheepGiveAmount
        {
            get => game.SheepGiveAmount;
            set
            {
                if (Convert.ToInt32(value) > game.PlayerSheepCount)
                    game.SheepGiveAmount = game.PlayerSheepCount.ToString();
                else
                    game.SheepGiveAmount = value;
                OnPropertyChanged(nameof(SheepGiveAmount));
            }
        }
        public string WheatGiveAmount
        {
            get => game.WheatGiveAmount;
            set
            {
                if (Convert.ToInt32(value) > game.PlayerWheatCount)
                    game.WheatGiveAmount = game.PlayerWheatCount.ToString();
                else
                    game.WheatGiveAmount = value;
                OnPropertyChanged(nameof(WheatGiveAmount));
            }
        }
        public string OreGiveAmount
        {
            get => game.OreGiveAmount;
            set
            {
                if (Convert.ToInt32(value) > game.PlayerOreCount)
                    game.OreGiveAmount = game.PlayerOreCount.ToString();
                else
                    game.OreGiveAmount = value;
                OnPropertyChanged(nameof(OreGiveAmount));
            }
        }
        public string WoodGetAmount
        {
            get => game.WoodGetAmount;
            set
            {
                if(Convert.ToInt32(value) > 25)
                    game.WoodGetAmount = 25.ToString();
                else
                    game.WoodGetAmount = value;
                OnPropertyChanged(nameof(WoodGetAmount));
            }
        }
        public string BrickGetAmount
        {
                get => game.BrickGetAmount;
                set
                {
                    if (Convert.ToInt32(value) > 25)
                        game.BrickGetAmount = 25.ToString();
                    else
                        game.BrickGetAmount = value;
                    OnPropertyChanged(nameof(BrickGetAmount));
                }
        }
        public string SheepGetAmount
        {
            get => game.SheepGetAmount;
            set
            {
                if (Convert.ToInt32(value) > 25)
                    game.SheepGetAmount = 25.ToString();
                else
                    game.SheepGetAmount = value;
                OnPropertyChanged(nameof(SheepGetAmount));
            }
        }
        public string WheatGetAmount
        {
            get => game.WheatGetAmount;
            set
            {
                if (Convert.ToInt32(value) > 25)
                    game.WheatGetAmount = 25.ToString();
                else
                    game.WheatGetAmount = value;
                OnPropertyChanged(nameof(WheatGetAmount));
            }
        }
        public string OreGetAmount
        {
            get => game.OreGetAmount;
            set
            {
                if (Convert.ToInt32(value) > 25)
                    game.OreGetAmount = 25.ToString();
                else
                    game.OreGetAmount = value;
                OnPropertyChanged(nameof(OreGetAmount));
            }
        }

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
            ConfirmTradeWithPlayerCommand = new Command(ConfirmTradeWithPlayer);
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
