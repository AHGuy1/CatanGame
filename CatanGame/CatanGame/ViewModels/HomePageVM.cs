using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class HomePageVM : ObservableObject
    {
        private readonly Games games = new();
        public bool IsBusy => games.IsBusy;
        public string GameCodes { get; set; } = string.Empty;
        public string GameCode
        {
            get => GameCodes;
            set
            {      
                GameCodes = value;
                (JoinGameWithCodeCommand as Command)?.ChangeCanExecute();
            }
        }
        public static ObservableCollection<int> AmountOfPointsNeeded => Games.AmountOfPointsNeeded;
        public static string DisplayName => string.Empty;
        public int SlectedAmountOfPointsNeeded { get; set; }
        public ICommand JoinGameWithCodeCommand { get; }
        public ICommand AddGameCommand { get; }
        public ObservableCollection<GameSize>? AmountOfPlayers { get => games.AmountOfPlayers; set => games.AmountOfPlayers = value; }
        public ObservableCollection<TurnTime> TurnTimes { get => games.TurnTimes; set => games.TurnTimes = value; }
        public ObservableCollection<Game>? GamesList => games.GamesList;
        public GameSize SlectedAmountOfPlayers { get; set; } = new GameSize();
        public TurnTime SelectedTurnTime { get; set; } = new TurnTime();
        public Game? SelectedItem
        {
            get => games.CurrentGame;

            set
            {
                if (value != null)
                {
                    games.CurrentGame = value;
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.Navigation.PushAsync(new WaitingRoomPage(value), true);
                    });
                }
            }
        }

        public HomePageVM()
        {
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
            JoinGameWithCodeCommand = new Command(JoinGameWithCode, CanJoinGameWithCode);
            AddGameCommand = new Command(AddGame);
        }

        private void AddGame()
        {
            games.AddGame(SlectedAmountOfPlayers,SlectedAmountOfPointsNeeded,SelectedTurnTime.Time);
            OnPropertyChanged(nameof(IsBusy));
        }

        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }

        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new WaitingRoomPage(game), true);
            });
        }
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }

        private bool CanJoinGameWithCode()
        {
            return !String.IsNullOrEmpty(GameCode) && int.Parse(GameCode) > 100000 && int.Parse(GameCode) < 1000000;
        }

        private void JoinGameWithCode()
        {
            games.JoinGameWithCode(GameCode);
        }
    }
}
