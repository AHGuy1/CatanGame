using CatanGame.Models;
using CatanGame.ModelsLogic;
using System.Windows.Input;

namespace CatanGame.ViewModels
{
    public partial class EndGamePageVM : ObservableObject
    {
        #region Fields
        private Game Game { get; set; }
        private readonly ModelsLogic.Connectivity connectivity = new();
        #endregion

        #region Commands
        public ICommand ReturnToHomeCommand { get;}
        #endregion

        #region Properties
        public string GameResult => Game.PlayerIndicator == Game.WinnerIndecator ? Strings.YouWonLabel : Strings.YouLostLabel;
        public string PointsEarnd => Strings.FinishedWith + Game.PlayerPoints + Strings.EmptySpace + Strings.VictoryPoints;
        public string GoalReachedMessage => (Game.PlayerIndicator == Game.WinnerIndecator ? Strings.YouReachedTheGoal : Game.PlayerNames[Game.WinnerIndecator] 
            + Strings.EmptySpace + Strings.HasReachedTheGoal) + Strings.EmptySpace + Game.PointsGoal + Strings.EmptySpace+ Strings.VictoryPoints;
        public bool PlayerLost => Game.PlayerIndicator != Game.WinnerIndecator;
        public bool IsDisconnected => !connectivity.IsConnected;
        public Color GameResultColor  => Game.PlayerIndicator == Game.WinnerIndecator ? Colors.Green : Colors.Red;
        #endregion

        #region Constructor
        public EndGamePageVM(Game game) 
        {
            Game = game;
            ReturnToHomeCommand = new Command(ReturnToHome);
            connectivity.ConnectivityChanged += OnConnectivityChanged;
        }
        #endregion

        #region Private Methods
        //Updates xaml that connectivity status has changed.
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsDisconnected));
        }

        // Returns to the app home screen.
        private void ReturnToHome()
        {
            if (Application.Current != null)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
        }
        #endregion
    }
}
