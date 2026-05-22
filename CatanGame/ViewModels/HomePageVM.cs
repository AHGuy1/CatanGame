using CatanGame.Models;
using CatanGame.ModelsLogic;
using CatanGame.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class HomePageVM : ObservableObject
    {
        #region Fields
        private readonly Games games = new();
        private string GameCodePrivate = string.Empty;
        private string SelectedBoardTypePrivate = string.Empty;
        #endregion

        #region Commands
        public ICommand JoinGameWithCodeCommand { get; }
        public ICommand AddGameCommand { get; }
        #endregion

        #region Properties
        public bool IsRandomBoard { get; set; }
        public bool IsBusy => games.IsBusy;
        public bool IsEnabled => !IsBusy;
        public static ObservableCollection<int> AmountOfPointsNeeded => Games.AmountOfPointsNeeded;
        public static ObservableCollection<string> BoardTypes => Games.BoardTypes;
        public static string DisplayName => string.Empty;
        public int SlectedAmountOfPointsNeeded { get; set; }
        public ObservableCollection<GameSize>? AmountOfPlayers { get => games.AmountOfPlayers; set => games.AmountOfPlayers = value; }
        public ObservableCollection<TurnTime> TurnTimes { get => games.TurnTimes; set => games.TurnTimes = value; }
        public ObservableCollection<Game>? GamesList => games.GamesList;
        public GameSize SlectedAmountOfPlayers { get; set; } = new GameSize();
        public TurnTime SelectedTurnTime { get; set; } = new TurnTime();
        public string GameCode
        {
            get => GameCodePrivate;
            set
            {
                GameCodePrivate = value;
                (JoinGameWithCodeCommand as Command)?.ChangeCanExecute();
            }
        }
        public string SelectedBoardType
        {
            get => SelectedBoardTypePrivate;
            set
            {
                IsRandomBoard = value == Strings.RandomBoardLabel;
                SelectedBoardTypePrivate = value;
            }
        }
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
        #endregion

        #region Constructor
        public HomePageVM()
        {
            games.GameAdded += OnGameAdded;
            games.GamesChanged += OnGamesChanged;
            JoinGameWithCodeCommand = new Command(JoinGameWithCode, CanJoinGameWithCode);
            AddGameCommand = new Command(AddGame);
        }
        #endregion

        #region Public Methods
        // Starts listening for available games.
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }

        // Stops listening for available games.
        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }
        #endregion

        #region Private Methods
        // Creates a game with the selected settings.
        private void AddGame()
        {
            games.AddGame(SlectedAmountOfPlayers, SlectedAmountOfPointsNeeded, SelectedTurnTime.Time, IsRandomBoard);
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsEnabled));
        }

        // Refreshes the displayed games list.
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }

        // Opens the waiting room after game creation.
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsEnabled));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new WaitingRoomPage(game), true);
            });
        }

        // Checks whether the entered game code is valid.
        private bool CanJoinGameWithCode()
        {
            return !String.IsNullOrEmpty(GameCode) && int.Parse(GameCode) > 100000 && int.Parse(GameCode) < 1000000;
        }

        // Starts joining by game code.
        private void JoinGameWithCode()
        {
            games.JoinGameWithCode(GameCode);
        }
        #endregion
    }
}
