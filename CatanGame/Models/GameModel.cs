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
        public EventHandler<string>? PlayerLeft;
        #endregion

        #region Properties
        [Ignored]
        public Avatar PlayerAvatar { get; set; } = new Avatar();
        [Ignored]
        public string StatusMessage => _status.StatusMessage;
        [Ignored]
        public Color StatusColor { get; set; } = Colors.Black;
        [Ignored]
        public Board GameBoard { get; } = new();
        [Ignored]
        public int PlayerLongestRoadLength { get; set; }
        [Ignored]
        public int PlayerArmySize { get; set; }
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
            + (LargestArmyOwnerIndexe == PlayerIndicator ? 2 : 0);
        [Ignored]
        public int PlayerOreCount { get; set; }
        [Ignored]
        public int PlayerBrickCount { get; set; }
        [Ignored]
        public int PlayerWoodCount { get; set; }
        [Ignored]
        public int PlayerWheatCount { get; set; }
        [Ignored]
        public int PlayerSheepCount { get; set; }
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
        public int LongestRoadOwnerIndex { get; set; } = -1;
        public int LargestArmySize { get; set; } = 2;
        public int LargestArmyOwnerIndexe { get; set; } = -1;
        public int TurnTime { get; set; }
        public int Turn { get; set; } = 1;
        public int PointsGoal { get; set; }
        public int PlayerTurn { get; set; } = 1;
        public int PlayerCount { get; set; }
        public int MonoplizedCardsCount { get; set; }
        public int PlayersPassed { get; set; }
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
        // Saves the game document.
        public abstract void SetDocument(Action<Task> OnComplete);
        // Deletes the game document.
        public abstract void DeleteDocument(Action<Task> OnComplete);
        // Updates game fields and reports completion.
        public abstract void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict);
        // Updates selected game fields.
        public abstract void UpdateFields(Dictionary<string, object> dict);
        // Loads a game document.
        public abstract void GetDocument(string GameCode, Action<IDocumentSnapshot> OnComplete);
        // Starts a bank trade.
        public abstract void TradeWithBank(object parameter);
        // Selects a card to receive.
        public abstract void PickCardToGet(object paramater);
        // Closes the trade flow.
        public abstract void CloseTrade();
        // Accepts a trade offer.
        public abstract void AcceptTrade();
        // Declines a trade offer.
        public abstract void DeclineTrade();
        // Sends a player trade offer.
        public abstract void ConfirmTradeWithPlayer();
        // Completes a bank trade.
        public abstract void ConfirmTradeWithBank();
        // Cancels a trade request.
        public abstract void CancelTradeRequest();
        // Creates a counter offer.
        public abstract void CounterOffer();
        // Allocates resources from a dice roll.
        public abstract void AllocateResources();
        // Allocates starting settlement resources.
        public abstract void AllocateStartingResources(int row, int column);
        // Starts the game.
        public abstract void StartGame();
        // Adds the current player name.
        public abstract void AddPlayerName();
        // Ends the current turn.
        public abstract void EndTurn();
        // Removes the game listener.
        public abstract void RemoveSnapshotListener();
        // Adds the game listener.
        public abstract void AddSnapshotListener();
        // Checks whether a player trade can be sent.
        public abstract bool CenTradeWithPlayer();
        // Checks whether a trade offer can be accepted.
        public abstract bool CenAcceptTrade();
        // Gets players available for trading.
        public abstract string[] GetPlayersToTradeWith();
        #endregion

        #region PrivateMethods
        // Gets the display color for the active player.
        protected static Color GetStatusColor(int playerTurn) { if(playerTurn > 6) return Colors.Black; return Colors.White; }

        // Clears game event handlers.
        protected abstract void ClearEventHandelers();
        // Initializes avatar options.
        protected abstract void InitAvatar();
        // Updates status text and color.
        protected abstract void UpdateStatus();
        // Applies remote document changes.
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        // Handles a player leaving before start.
        protected abstract void OnCompletePlayerLeft(Task task);
        // Handles game deletion completion.
        protected abstract void OnCompleteDeleted(Task task);
        // Handles add player completion.
        protected abstract void OnCompleteAddPlayerName(Task task);
        // Handles turn update completion.
        protected abstract void OnTurnChanged(Task task);
        // Handles timer messages.
        protected abstract void OnMessageReceived(long timeleft);
        // Handles a trade response.
        protected abstract void CheckTradeResponce();
        // Resets trade state.
        protected abstract void ResetTradeParameters();
        // Handles a counter offer.
        protected abstract void ReciveCounterOffer();
        // Starts the turn timer.
        protected abstract void StartTimer();
        // Registers timer message handling.
        protected abstract void RegisterTimer();
        // Stops the turn timer.
        protected abstract void StopTimer();
        // Initializes board piece sources.
        protected abstract void IntArrayBoardPieces();
        // Clears selected card styling.
        protected abstract void ResetSelctedCardBorder();
        // Shows a trade message.
        protected abstract void ShowTradeAlert();
        // Syncs trade parameters.
        protected abstract void UpdateTradeParamaters();
        // Transfers trade resources.
        protected abstract void AllocateTradeResources();
        // Notifies that a trade was received.
        protected abstract void RecivedTrade();
        #endregion
    }
}
