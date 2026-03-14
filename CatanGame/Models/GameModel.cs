using Android.Media;
using CatanGame.ModelsLogic;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Timers;

namespace CatanGame.Models
{
    public abstract class GameModel
    {
        #region Fields
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        #endregion

        #region Events
        [Ignored]
        public EventHandler? TimeLeftChanged;
        [Ignored]
        public EventHandler? EndTurnOutOfTime;
        [Ignored]
        public EventHandler? GameChanged;
        [Ignored]
        public EventHandler? GridChanged;
        [Ignored]
        public EventHandler? TurnChanged;
        [Ignored]
        public EventHandler? AnimationStatusChanged;
        [Ignored]
        public EventHandler? ResourceCountersUpdated;
        [Ignored]
        public EventHandler? TradeRecived;
        [Ignored]
        public EventHandler? CloseTradePopUp;
        [Ignored]
        public EventHandler<string>? GameDeleted;
        [Ignored]
        public EventHandler<int>? PlayerLeft;
        #endregion

        #region Properties
        [Ignored]
        public Avatar PlayerAvatar { get; set; } = new Avatar();
        [Ignored]
        public abstract GameStatus Status { get; }
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        [Ignored]
        public Color StatusColor { get; set; } = Colors.Black;
        [Ignored]
        public Board GameBoard { get; } = new();
        [Ignored]
        public int PlayerLongestRoadLength { get; set; }
        [Ignored]
        public int PlayerLargestArmySize { get; set; }
        [Ignored]
        public int PlayerIndicator { get; set; }
        [Ignored]
        public int RollTotal => Roll1 + Roll2;
        [Ignored]
        public int PlayerCityCount { get; set; }
        [Ignored]
        public int PlayerTownCount { get; set; }
        [Ignored]
        public int PlayerRoadCount { get; set; }
        [Ignored]
        public int PlayerPoints =>
            PlayerTownCount
            + (PlayerCityCount * 2)
            + PlayerVictoryPointCardsCount
            + (LongestRoadOwnerIndex == PlayerIndicator ? 2 : 0)
            + (LargestArmySize == PlayerLargestArmySize ? 2 : 0);
        [Ignored]
        public int PlayerOreCount { get; set; } = 5;
        [Ignored]
        public int PlayerBrickCount { get; set; } = 5;
        [Ignored]
        public int PlayerWoodCount { get; set; } = 5;
        [Ignored]
        public int PlayerWheatCount { get; set; } = 5;
        [Ignored]
        public int PlayerSheepCount { get; set; } = 5;
        [Ignored]
        public int PlayerVictoryPointCardsCount { get; set; }
        [Ignored]
        public bool IsRandomBoard { get; set; }
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        [Ignored]
        // Index 0 = 3:1, 1 = Wood, 2 = Brick, 3 = Sheep, 4 = Wheat, 5 = Ore
        public bool[] PlayerOwnedHarbors { get; set; } = new bool[6];
        [Ignored]
        public string SelectedTradeCard { get; set; } = string.Empty;
        [Ignored]
        public ImageButton? PreviselySelctedCard { get; set; }
        public int WinnerIndecator { get; set; } = -1;
        public int Roll1 { get; set; }
        public int Roll2 { get; set; }
        public int LongestRoadLength { get; set; } = 4;
        public int LongestRoadOwnerIndex { get; set; } = 0;
        public int LargestArmySize { get; set; } = 2;
        public int TurnTime { get; set; }
        public int Turn { get; set; } = 1;
        public int PointsGoal { get; set; }
        public int PlayerTurn { get; set; } = 1;
        public int PlayerCount { get; set; }
        public int MonoplizedCardsCount { get; set; }
        public int PlayersPassed { get; set; }
        public bool Rold { get; set; }
        public bool IsFull { get; set; }
        public bool IsRolling { get; set; }
        public bool GameStarted { get; set; }
        public bool TradeInProgress { get; set; } = false;
        public string TradeMessage { get; set; } = string.Empty;
        public string GameCode { get; set; } = string.Empty;
        public string WoodTradeGiveAmount { get; set; } = string.Empty;
        public string BrickTradeGiveAmount { get; set; } = string.Empty;
        public string SheepTradeGiveAmount { get; set; } = string.Empty;
        public string WheatTradeGiveAmount { get; set; } = string.Empty;
        public string OreTradeGiveAmount { get; set; } = string.Empty;
        public string WoodTradeGetAmount { get; set; } = string.Empty;
        public string BrickTradeGetAmount { get; set; } = string.Empty;
        public string SheepTradeGetAmount { get; set; } = string.Empty;
        public string WheatTradeGetAmount { get; set; } = string.Empty;
        public string OreTradeGetAmount { get; set; } = string.Empty;
        public string SelectedPlayerName = string.Empty;
        public string MonopolizedCard { get; set; } = string.Empty;
        public string MonoplizingPlayer { get; set; } = string.Empty;
        public int[] RobberPlacment { get; set; } = new int[2];
        public string[] SpecialCards { get; set; } = [];
        public string[] PlayersInTrade { get; set; } = new string[2];
        public string[] TileNumbers { get; set; } = new string[19];
        public string[] TileTypes { get; set; } = new string[19];
        public string[] BoardPieces { get; set; } = new string[276];
        public string[] PlayerNames { get; set; } = [string.Empty];
        public DateTime Created { get; set; }
        #endregion

        #region PublicMethods
        public abstract void SetDocument(Action<Task> OnComplete);
        public abstract void DeleteDocument(Action<Task> OnComplete);
        public abstract void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict);
        public abstract void UpdateFields(Dictionary<string, object> dict);
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
        public abstract void TradeWithBank(object parameter);
        public abstract void PickCardToGet(object paramater);
        public abstract void CloseTrade();
        public abstract void AcceptTrade();
        public abstract void DeclineTrade();
        public abstract void ConfirmTradeWithPlayer();
        public abstract void ConfirmTradeWithBank();
        public abstract void CancelTradeRequest();
        public abstract void CounterOffer();
        public abstract void AllocateResources();
        public abstract void StartGame();
        public abstract void AddPlayerName();
        public abstract void EndTurn();
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract bool CenTradeWithPlayer();
        public abstract bool CenAcceptTrade();
        public abstract string[] GetPlayersToTradeWith();
        #endregion

        #region PrivateMethods
        protected abstract void InitAvatar();
        protected abstract void UpdateStatus();
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        protected abstract void OnCompletePlayerLeft(Task task);
        protected abstract void OnCompleteDeleted(Task task);
        protected abstract void OnCompleteAddPlayerName(Task task);
        protected abstract void OnTurnChanged(Task task);
        protected abstract void OnMessageReceived(long timeleft);
        protected abstract void CheckTradeResponce();
        protected abstract void ResetTradeParameters();
        protected abstract void ReciveCounterOffer();
        protected abstract void StartTimer();
        protected abstract void RegisterTimer();
        protected abstract void StopTimer();
        protected abstract void IntArrayBoardPieces();
        protected abstract void ResetSelctedCardBorder();
        protected abstract void ShowTradeAlert();
        protected abstract void UpdateTradeParamaters();
        protected abstract void AllocateTradeResources();
        protected abstract void RecivedTrade();
        #endregion
    }
}
