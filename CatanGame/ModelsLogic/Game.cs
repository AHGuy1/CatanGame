using CatanGame.Models;
using CatanGame.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;

namespace CatanGame.ModelsLogic
{
    public class Game : GameModel
    {
        #region Properties
        #endregion

        #region Constructor
        public Game(GameSize slectedAmountOfPlayers, int selectedAmountOfPoints, int turnTime, bool isRandomBoard)
        {
            TurnTime = turnTime;
            IsRandomBoard = isRandomBoard;
            PlayerCount = slectedAmountOfPlayers.Size;
            PointsGoal = selectedAmountOfPoints;
            PlayerNames = new string[PlayerCount];
            Created = DateTime.Now;
            for (int i = 0; i < 2; i++)
                RobberPlacment[i] = -1;
            UpdateStatus();
            IntArrayBoardPieces();
            InitAvatar();
        }

        public Game()
        {
            IntArrayBoardPieces();
            InitAvatar();
        }
        #endregion

        #region Private Methods
        // Gets the display color for the active player.
        protected new static Color GetStatusColor(int playerTurn)
        {
            return playerTurn switch
            {
                1 => Colors.DarkOrange,
                2 => Colors.Navy,
                3 => Colors.Gold,
                4 => Colors.Red,
                5 => Colors.LimeGreen,
                6 => Colors.Cyan,
                _ => Colors.Black
            };
        }

        // Sets the avatar parts available to players.
        protected override void InitAvatar()
        {
            PlayerAvatar.SelectedEyes =
            [
                AvatarModel.Eyes.Bulging, AvatarModel.Eyes.Dizzy, AvatarModel.Eyes.Eva, AvatarModel.Eyes.Frame1, AvatarModel.Eyes.Frame2,
                    AvatarModel.Eyes.Glow, AvatarModel.Eyes.Robocop, AvatarModel.Eyes.Round, AvatarModel.Eyes.RoundFrame01,
                    AvatarModel.Eyes.RoundFrame02, AvatarModel.Eyes.Sensor, AvatarModel.Eyes.Shade01
            ];
            PlayerAvatar.SelectedMouths =
            [
                AvatarModel.Mouth.Bite, AvatarModel.Mouth.Diagram, AvatarModel.Mouth.Grill01, AvatarModel.Mouth.Grill02,
                    AvatarModel.Mouth.Grill03, AvatarModel.Mouth.Square01, AvatarModel.Mouth.Square02
            ];
            PlayerAvatar.SelectedFaces =
            [
                AvatarModel.Face.Round01, AvatarModel.Face.Round02, AvatarModel.Face.Square01, AvatarModel.Face.Square02
            ];
            PlayerAvatar.SelectedColors =
            [
                AvatarModel.Colors.OrangeRed, AvatarModel.Colors.Orange, AvatarModel.Colors.Indigo, AvatarModel.Colors.Cyan,
                    AvatarModel.Colors.BlueGrey, AvatarModel.Colors.Blue, AvatarModel.Colors.Brown, AvatarModel.Colors.Green,
                    AvatarModel.Colors.YellowGreen, AvatarModel.Colors.Yellow, AvatarModel.Colors.Red, AvatarModel.Colors.LightGreen,
                    AvatarModel.Colors.LightBlue, AvatarModel.Colors.Grey, AvatarModel.Colors.Amber, AvatarModel.Colors.Teal,
                    AvatarModel.Colors.Pink
            ];
            PlayerAvatar.SelectedTops =
            [
                AvatarModel.Top.Antenna, AvatarModel.Top.AntennaCrooked, AvatarModel.Top.Bulb01, AvatarModel.Top.GlowingBulb01,
                    AvatarModel.Top.GlowingBulb02, AvatarModel.Top.Lights, AvatarModel.Top.Pyramid, AvatarModel.Top.Radar
            ];
        }

        // Clears the board piece image sources.
        protected override void IntArrayBoardPieces()
        {
            for (int i = 0; i < 276; i++)
                BoardPieces[i] = string.Empty;
        }

        // Subscribes this game to timer messages.
        protected override void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) => OnMessageReceived(m.Value));
        }

        // Updates the displayed turn timer from timer messages.
        protected override void OnMessageReceived(long timeleft)
        {
            if (timeleft == Keys.FinishedSignal)
            {
                TimeLeft = Strings.TimeUp;
                if (PlayerTurn == PlayerIndicator + 1)
                    EndTurnOutOfTime?.Invoke(this, EventArgs.Empty);
            }
            else if ((double)timeleft / 1000 <= 10.0)
            {
                TimeLeft = double.Round(((double)timeleft / 1000), 1).ToString();
                TimeLeftChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TimeLeft = double.Round(((double)timeleft / 1000), 0).ToString();
                TimeLeftChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Starts the turn timer.
        protected override void StartTimer()
        {
            TimerSettings ts = new((TurnTime * 1000) + 1, 10);
            WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(ts));
        }

        // Stops the turn timer.
        protected override void StopTimer()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<string>(Keys.StopSignal));
        }

        // Notifies the UI after a waiting room player leaves.
        protected override void OnCompletePlayerLeft(Task task)
        {
            string message = "";
            if (PlayerIndicator == 1)
                message = Strings.Player2Left;
            else if (PlayerIndicator == 2)
                message = Strings.Player3Left;
            else if (PlayerIndicator == 3)
                message = Strings.Player4Left;
            else if (PlayerIndicator == 4)
                message = Strings.Player5Left;
            else if (PlayerIndicator == 5)
                message = Strings.Player6Left;
            PlayerLeft?.Invoke(this, message);
        }

        // Applies remote game document changes to this game.
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null && WinnerIndecator == -1)
            {
                for (int i = 1; i < PlayerCount; i++)
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                    {
                        for (int j = 1; j < PlayerCount; j++)
                            if (PlayerNames[j] != updatedGame.PlayerNames[j])
                            {
                                string message = "";
                                if (j == 1)
                                    message = Strings.Player2Left;
                                else if (j == 2)
                                    message = Strings.Player3Left;
                                else if (j == 3)
                                    message = Strings.Player4Left;
                                else if (j == 4)
                                    message = Strings.Player5Left;
                                else if (j == 5)
                                    message = Strings.Player6Left;
                                PlayerLeft?.Invoke(this, message);
                                if (j < PlayerIndicator)
                                    PlayerIndicator--;
                                j = PlayerCount;
                            }
                        i = PlayerCount;
                    }
                IsFull = updatedGame.IsFull;
                PlayerNames = updatedGame.PlayerNames;
                TurnTime = updatedGame.TurnTime;
                Roll1 = updatedGame.Roll1;
                Roll2 = updatedGame.Roll2;
                SpecialCards = updatedGame.SpecialCards;
                if(WinnerIndecator != updatedGame.WinnerIndecator)
                {
                    ClearEventHandelers();
                    StopTimer();
                    WinnerIndecator = updatedGame.WinnerIndecator;
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Application.Current!.MainPage = new EndGamePage(this);
                    });
                }
                else
                {
                    if (TradeMessage != updatedGame.TradeMessage)
                    {
                        TradeMessage = updatedGame.TradeMessage;
                        if (TradeMessage != Strings.TradeDeclined && TradeMessage != Strings.TradeAccepted && TradeMessage != Strings.PlayerCounterOffer && TradeMessage != Strings.TradeCanceled)
                        {
                            if (TradeMessage != string.Empty)
                            {
                                ShowTradeAlert();
                                TradeMessage = string.Empty;
                            }
                        }
                        else if (TradeInProgress && updatedGame.PlayersInTrade[0] == PlayerNames[PlayerIndicator])
                            CheckTradeResponce();
                    }
                    else if (TradeMessage.Contains(PlayerNames[PlayerIndicator]))
                    {
                        TradeMessage = string.Empty;
                        Dictionary<string, object> dict = new()
                        {
                            { nameof(TradeMessage), TradeMessage }
                        };
                        UpdateFields(dict);
                    }
                    WoodTradeGetAmount = updatedGame.WoodTradeGetAmount;
                    BrickTradeGetAmount = updatedGame.BrickTradeGetAmount;
                    SheepTradeGetAmount = updatedGame.SheepTradeGetAmount;
                    WheatTradeGetAmount = updatedGame.WheatTradeGetAmount;
                    OreTradeGetAmount = updatedGame.OreTradeGetAmount;
                    WoodTradeGiveAmount = updatedGame.WoodTradeGiveAmount;
                    BrickTradeGiveAmount = updatedGame.BrickTradeGiveAmount;
                    SheepTradeGiveAmount = updatedGame.SheepTradeGiveAmount;
                    WheatTradeGiveAmount = updatedGame.WheatTradeGiveAmount;
                    OreTradeGiveAmount = updatedGame.OreTradeGiveAmount;
                    if (TradeMessage == Strings.PlayerCounterOffer && TradeInProgress && updatedGame.PlayersInTrade[1] == PlayerNames[PlayerIndicator])
                    {
                        PlayersInTrade = updatedGame.PlayersInTrade;
                        ReciveCounterOffer();
                    }
                    if (TradeInProgress != updatedGame.TradeInProgress)
                    {
                        TradeInProgress = updatedGame.TradeInProgress;
                        if (TradeInProgress && updatedGame.PlayersInTrade[1] == PlayerNames[PlayerIndicator])
                        {
                            PlayersInTrade = updatedGame.PlayersInTrade;
                            RecivedTrade();
                        }
                        else if (!TradeInProgress && PlayersInTrade[1] == PlayerNames[PlayerIndicator])
                        {
                            PlayersInTrade = updatedGame.PlayersInTrade;
                            CloseTrade();
                        }
                    }
                    PlayersInTrade = updatedGame.PlayersInTrade;
                    if (TileTypes[0] == null)
                    {
                        TileNumbers = updatedGame.TileNumbers;
                        TileTypes = updatedGame.TileTypes;
                    }
                    bool gridChanged = false;
                    if (MonoplizingPlayer != updatedGame.MonoplizingPlayer && updatedGame.MonoplizingPlayer != string.Empty && updatedGame.MonoplizingPlayer != PlayerNames[PlayerIndicator])
                    {
                        MonoplizingPlayer = updatedGame.MonoplizingPlayer;
                        MonopolizedCard = updatedGame.MonopolizedCard;
                        int takenCount = 0;
                        if (MonopolizedCard == Strings.WoodImage)
                        {
                            takenCount = PlayerWoodCount;
                            PlayerWoodCount = 0;
                        }
                        else if (MonopolizedCard == Strings.BrickImage)
                        {
                            takenCount = PlayerBrickCount;
                            PlayerBrickCount = 0;
                        }
                        else if (MonopolizedCard == Strings.SheepImage)
                        {
                            takenCount = PlayerSheepCount;
                            PlayerSheepCount = 0;
                        }
                        else if (MonopolizedCard == Strings.WheatImage)
                        {
                            takenCount = PlayerWheatCount;
                            PlayerWheatCount = 0;
                        }
                        else if (MonopolizedCard == Strings.OreImage)
                        {
                            takenCount = PlayerOreCount;
                            PlayerOreCount = 0;
                        }
                        Dictionary<string, object> dict = new()
                        {
                            { nameof(PlayersPassed), FieldValue.Increment(1) },
                            { nameof(MonoplizedCardsCount), FieldValue.Increment(takenCount) }
                        };
                        UpdateFields(dict);
                        gridChanged = true;
                    }
                    if(updatedGame.MonoplizingPlayer == PlayerNames[PlayerIndicator])
                    {
                        PlayersPassed = updatedGame.PlayersPassed;
                        MonoplizedCardsCount = updatedGame.MonoplizedCardsCount;
                    }
                    if (PlayersPassed == PlayerCount && MonoplizingPlayer == PlayerNames[PlayerIndicator])
                    {
                        if (MonopolizedCard == Strings.WoodImage)
                            PlayerWoodCount += MonoplizedCardsCount;
                        else if (MonopolizedCard == Strings.BrickImage)
                            PlayerBrickCount += MonoplizedCardsCount;
                        else if (MonopolizedCard == Strings.SheepImage)
                            PlayerSheepCount += MonoplizedCardsCount;
                        else if (MonopolizedCard == Strings.WheatImage)
                            PlayerWheatCount += MonoplizedCardsCount;
                        else if (MonopolizedCard == Strings.OreImage)
                            PlayerOreCount += MonoplizedCardsCount;
                        MonoplizingPlayer = string.Empty;
                        MonopolizedCard = string.Empty;
                        MonoplizedCardsCount = 0;
                        PlayersPassed = 0;
                        Dictionary<string, object> dict = new()
                        {
                            { nameof(PlayersPassed), PlayersPassed },
                            { nameof(MonopolizedCard), MonopolizedCard },
                            { nameof(MonoplizingPlayer), MonoplizingPlayer }
                        };
                        UpdateFields(dict);
                        gridChanged = true;
                    }
                    if (Turn != updatedGame.Turn)
                    {
                        PlayerTurn = updatedGame.PlayerTurn;
                        Turn = updatedGame.Turn;
                        if (WinnerIndecator == -1)
                        {
                            TurnChanged?.Invoke(this, EventArgs.Empty);
                            gridChanged = true;
                            StartTimer();
                        }
                    }
                    if (PlayerTurn == PlayerIndicator + 2)
                        gridChanged = true;
                    for (int i = 0; i < BoardPieces.Length; i++)
                        if (BoardPieces[i] != updatedGame.BoardPieces[i])
                        {
                            gridChanged = true;
                            BoardPieces[i] = updatedGame.BoardPieces[i];
                        }
                    if (LongestRoadLength != updatedGame.LongestRoadLength || LongestRoadOwnerIndex != updatedGame.LongestRoadOwnerIndex ||
                        LargestArmySize != updatedGame.LargestArmySize || LargestArmyOwnerIndexe != updatedGame.LargestArmyOwnerIndexe)
                    {
                        LongestRoadLength = updatedGame.LongestRoadLength;
                        LongestRoadOwnerIndex = updatedGame.LongestRoadOwnerIndex;
                        LargestArmySize = updatedGame.LargestArmySize;
                        LargestArmyOwnerIndexe = updatedGame.LargestArmyOwnerIndexe;
                        gridChanged = true;
                    }
                    if (RobberPlacment[0] != updatedGame.RobberPlacment[0] || RobberPlacment[1] != updatedGame.RobberPlacment[1])
                    {
                        RobberPlacment = updatedGame.RobberPlacment;
                        gridChanged = true;
                    }
                    if (IsRolling != updatedGame.IsRolling)
                    {
                        IsRolling = updatedGame.IsRolling;
                        AnimationStatusChanged?.Invoke(this, EventArgs.Empty);
                    }
                    if (gridChanged)
                        GridChanged?.Invoke(this, EventArgs.Empty);
                    if (updatedGame.GameStarted && !GameStarted)
                        MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            RegisterTimer();
                            StartTimer();
                            GameStarted = updatedGame.GameStarted;
                            Application.Current!.MainPage = new GamePage(this);
                        });
                    UpdateStatus();
                    GameChanged?.Invoke(this, EventArgs.Empty);
                }              
            }
            else if(WinnerIndecator == -1)
            {
                if (!GameStarted)
                    GameDeleted?.Invoke(this, Strings.HostLeft);
                else
                    GameDeleted?.Invoke(this, string.Empty);
            }
        }

        // Shows the current trade message as a toast.
        protected override void ShowTradeAlert()
        {
            MainThread.InvokeOnMainThreadAsync(() => Toast.Make(TradeMessage, ToastDuration.Long, 15).Show());
        }

        // Notifies listeners when delete completes successfully.
        protected override void OnCompleteDeleted(Task task)
        {
            if (task.IsCompletedSuccessfully)
                GameDeleted?.Invoke(this, string.Empty);
        }

        // Handles failures when adding this player to a game.
        protected override void OnCompleteAddPlayerName(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);
        }

        // Notifies listeners after a turn update completes.
        protected override void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                GameChanged?.Invoke(this, EventArgs.Empty);
        }

        // Updates the current status message and color.
        protected override void UpdateStatus()
        {
            Array status = Enum.GetValues(typeof(GameStatus.Status));
            _status.CurrentStatus = !GameStarted ? GameStatus.Status.PleseWait :
                PlayerTurn == PlayerIndicator + 1 ? GameStatus.Status.YourTurn :
                (GameStatus.Status)status.GetValue(PlayerTurn - 1)!;
            StatusColor = GetStatusColor(PlayerTurn);
        }

        // Removes the selection border from the previous trade card.
        protected override void ResetSelctedCardBorder()
        {
            if (PreviselySelctedCard != null)
                PreviselySelctedCard.BorderWidth = 0;
        }

        // Sends the current trade parameters to Firestore.
        protected override void UpdateTradeParamaters()
        {
            Dictionary<string, object> dict = new()
                {
                    { nameof(TradeInProgress), TradeInProgress },
                    { nameof(PlayersInTrade), PlayersInTrade },
                    { nameof(SelectedPlayerName), SelectedPlayerName },
                    { nameof(WoodTradeGiveAmount), WoodTradeGiveAmount },
                    { nameof(BrickTradeGiveAmount), BrickTradeGiveAmount },
                    { nameof(SheepTradeGiveAmount), SheepTradeGiveAmount },
                    { nameof(WheatTradeGiveAmount), WheatTradeGiveAmount },
                    { nameof(OreTradeGiveAmount), OreTradeGiveAmount },
                    { nameof(WoodTradeGetAmount), WoodTradeGetAmount },
                    { nameof(BrickTradeGetAmount), BrickTradeGetAmount },
                    { nameof(SheepTradeGetAmount), SheepTradeGetAmount },
                    { nameof(WheatTradeGetAmount), WheatTradeGetAmount },
                    { nameof(OreTradeGetAmount), OreTradeGetAmount },
                    { nameof(TradeMessage), TradeMessage }
                };
            UpdateFields(dict);
        }

        // Transfers resources between the two players in a trade.
        protected override void AllocateTradeResources()
        {
            if (PlayersInTrade[0] == PlayerNames[PlayerIndicator])
            {
                PlayerWoodCount -= Convert.ToInt32(WoodTradeGiveAmount);
                PlayerBrickCount -= Convert.ToInt32(BrickTradeGiveAmount);
                PlayerSheepCount -= Convert.ToInt32(SheepTradeGiveAmount);
                PlayerWheatCount -= Convert.ToInt32(WheatTradeGiveAmount);
                PlayerOreCount -= Convert.ToInt32(OreTradeGiveAmount);
                PlayerWoodCount += Convert.ToInt32(WoodTradeGetAmount);
                PlayerBrickCount += Convert.ToInt32(BrickTradeGetAmount);
                PlayerSheepCount += Convert.ToInt32(SheepTradeGetAmount);
                PlayerWheatCount += Convert.ToInt32(WheatTradeGetAmount);
                PlayerOreCount += Convert.ToInt32(OreTradeGetAmount);
            }
            else if (PlayersInTrade[1] == PlayerNames[PlayerIndicator])
            {
                PlayerWoodCount += Convert.ToInt32(WoodTradeGiveAmount);
                PlayerBrickCount += Convert.ToInt32(BrickTradeGiveAmount);
                PlayerSheepCount += Convert.ToInt32(SheepTradeGiveAmount);
                PlayerWheatCount += Convert.ToInt32(WheatTradeGiveAmount);
                PlayerOreCount += Convert.ToInt32(OreTradeGiveAmount);
                PlayerWoodCount -= Convert.ToInt32(WoodTradeGetAmount);
                PlayerBrickCount -= Convert.ToInt32(BrickTradeGetAmount);
                PlayerSheepCount -= Convert.ToInt32(SheepTradeGetAmount);
                PlayerWheatCount -= Convert.ToInt32(WheatTradeGetAmount);
                PlayerOreCount -= Convert.ToInt32(OreTradeGetAmount);
            }
        }

        // Notifies the UI that a trade was received.
        protected override void RecivedTrade()
        {
            TradeRecived?.Invoke(this, EventArgs.Empty);
        }
        // Clears game event subscriptions.
        protected override void ClearEventHandelers()
        {
            TimeLeftChanged = null;
            EndTurnOutOfTime = null;
            GameChanged = null;
            GridChanged = null;
            TurnChanged = null;
            AnimationStatusChanged = null;
            ResourceCountersUpdated = null;
            TradeRecived = null;
            CloseTradePopUp = null;
            GameDeleted = null;
            PlayerLeft = null;
        }
        // Handles an accepted, declined, canceled, or countered trade.
        protected override void CheckTradeResponce()
        {
            MainThread.InvokeOnMainThreadAsync(() => Toast.Make(TradeMessage, ToastDuration.Long, 18).Show());
            if (TradeMessage == Strings.TradeAccepted)
            {
                AllocateTradeResources();
                ResourceCountersUpdated?.Invoke(this, EventArgs.Empty);
                TradeMessage = PlayersInTrade[0] + Strings.EmptySpace + Strings.TradedWith + Strings.EmptySpace + PlayersInTrade[1];
                TradeMessage += Strings.EmptySpace + Strings.Reciveing;
                if (Convert.ToInt32(WoodTradeGetAmount) > 0)
                    TradeMessage += WoodTradeGetAmount + Strings.EmptySpace + Strings.WoodImage[..Strings.WoodImage.IndexOf('.')];
                if (Convert.ToInt32(BrickTradeGetAmount) > 0)
                    TradeMessage += BrickTradeGetAmount + Strings.EmptySpace + Strings.BrickImage[..Strings.BrickImage.IndexOf('.')];
                if (Convert.ToInt32(SheepTradeGetAmount) > 0)
                    TradeMessage += SheepTradeGetAmount + Strings.EmptySpace + Strings.SheepImage[..Strings.SheepImage.IndexOf('.')];
                if (Convert.ToInt32(WheatTradeGetAmount) > 0)
                    TradeMessage += WheatTradeGetAmount + Strings.EmptySpace + Strings.WheatImage[..Strings.WheatImage.IndexOf('.')];
                if (Convert.ToInt32(OreTradeGetAmount) > 0)
                    TradeMessage += OreTradeGetAmount + Strings.EmptySpace + Strings.OreImage[..Strings.OreImage.IndexOf('.')];
                TradeMessage += Strings.EmptySpace + Strings.Giving;
                if (Convert.ToInt32(WoodTradeGiveAmount) > 0)
                    TradeMessage += WoodTradeGiveAmount + Strings.EmptySpace + Strings.WoodImage[..Strings.WoodImage.IndexOf('.')];
                if (Convert.ToInt32(BrickTradeGiveAmount) > 0)
                    TradeMessage += BrickTradeGiveAmount + Strings.EmptySpace + Strings.BrickImage[..Strings.BrickImage.IndexOf('.')];
                if (Convert.ToInt32(SheepTradeGiveAmount) > 0)
                    TradeMessage += SheepTradeGiveAmount + Strings.EmptySpace + Strings.SheepImage[..Strings.SheepImage.IndexOf('.')];
                if (Convert.ToInt32(WheatTradeGiveAmount) > 0)
                    TradeMessage += WheatTradeGiveAmount + Strings.EmptySpace + Strings.WheatImage[..Strings.WheatImage.IndexOf('.')];
                if (Convert.ToInt32(OreTradeGiveAmount) > 0)
                    TradeMessage += OreTradeGiveAmount + Strings.EmptySpace + Strings.OreImage[..Strings.OreImage.IndexOf('.')];
                ResetTradeParameters();
                UpdateTradeParamaters();
            }
            else if (TradeMessage == Strings.TradeDeclined || TradeMessage == Strings.TradeCanceled)
            {
                TradeMessage = string.Empty;
                ResetTradeParameters();
                UpdateTradeParamaters();
            }
            if (TradeMessage == Strings.TradeCanceled)
                CloseTradePopUp?.Invoke(this, EventArgs.Empty);
        }

        // Closes the active trade popup.
        public override void CloseTrade()
        {
            CloseTradePopUp?.Invoke(this, EventArgs.Empty);
        }

        // Resets all trade state to defaults.
        protected override void ResetTradeParameters()
        {
            TradeInProgress = false;
            PlayersInTrade[0] = string.Empty;
            PlayersInTrade[1] = string.Empty;
            SelectedPlayerName = string.Empty;
            WoodTradeGiveAmount = Strings.Zero;
            BrickTradeGiveAmount = Strings.Zero;
            SheepTradeGiveAmount = Strings.Zero;
            WheatTradeGiveAmount = Strings.Zero;
            OreTradeGiveAmount = Strings.Zero;
            WoodTradeGetAmount = Strings.Zero;
            BrickTradeGetAmount = Strings.Zero;
            SheepTradeGetAmount = Strings.Zero;
            WheatTradeGetAmount = Strings.Zero;
            OreTradeGetAmount = Strings.Zero;
        }

        // Shows a counter offer and reopens the trade flow.
        protected override void ReciveCounterOffer()
        {
            MainThread.InvokeOnMainThreadAsync(() => Toast.Make(TradeMessage, ToastDuration.Long, 20).Show());
            RecivedTrade();
        }
        #endregion

        #region Public Methods
        // Gets all other players available for trading.
        public override string[] GetPlayersToTradeWith()
        {
            string[] playerNames = new string[PlayerNames.Length - 1];
            int count = 0;
            for (int i = 0; i < PlayerNames.Length; i++)
            {
                if (i != PlayerIndicator)
                {
                    playerNames[count] = PlayerNames[i];
                    count++;
                }
            }
            return playerNames;
        }

        // Saves this game document to Firestore.
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        // Updates game fields and runs a completion callback.
        public override void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        // Updates selected game fields.
        public override void UpdateFields(Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict);
        }

        // Loads a game document by id.
        public override void GetDocument(string Id, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GamesCollection, Id, OnComplete);
        }

        // Starts listening to this game document.
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        // Leaves the game and removes the snapshot listener.
        public override void RemoveSnapshotListener()
        {
            StopTimer();
            ilr?.Remove();
            PlayerNames[PlayerIndicator] = string.Empty;
            if (PlayerIndicator == 0 || GameStarted)
                DeleteDocument(OnCompleteDeleted);
            else if(!GameStarted)
            {
                for (int i = 0; i < PlayerCount - 1; i++)
                    if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                    {
                        PlayerNames[i] = PlayerNames[i + 1];
                        PlayerNames[i + 1] = string.Empty;
                    }
                IsFull = false;
                Dictionary<string, object> dict = new()
                    {
                        { nameof(IsFull), IsFull },
                        { nameof(PlayerNames), PlayerNames },
                    };
                UpdateFields(OnCompletePlayerLeft, dict);
            }
        }

        // Deletes the game and its join code.
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id);
            fbd.DeleteDocument(Keys.GameCodesCollection, GameCode, OnComplete);
        }

        // Ends the current player's turn and advances the game.
        public override void EndTurn()
        {
            if (PlayerTurn == PlayerCount)
                PlayerTurn = 1;
            else
                PlayerTurn++;
            Turn++;
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerTurn), PlayerTurn },
                { nameof(Turn), Turn }
            };
            if (PlayerPoints >= PointsGoal)
            {
                WinnerIndecator = PlayerIndicator;
                dict.Add(nameof(WinnerIndecator), WinnerIndecator);
                UpdateFields(dict);
                ClearEventHandelers();
                StopTimer();
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current!.MainPage = new EndGamePage(this);
                });
            }
            else
            {
                UpdateFields(OnTurnChanged, dict);
                GridChanged?.Invoke(this, EventArgs.Empty);
                StartTimer();
            }
        }

        // Starts the game for all joined players.
        public override void StartGame()
        {
            if (!GameStarted)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    RegisterTimer();
                    StartTimer();
                });
                GameStarted = true;
                Dictionary<string, object> dict = new()
                {
                    { nameof(GameStarted), GameStarted },
                };
                UpdateFields(OnTurnChanged, dict);
            }
        }

        // Adds the current user to the first open player slot.
        public override void AddPlayerName()
        {
            bool addedName = false;
            for (int i = 0; i < PlayerCount && !addedName; i++)
                if (String.IsNullOrWhiteSpace(PlayerNames[i]))
                {
                    PlayerNames[i] = fbd.DisplayName;
                    if (i + 1 == PlayerCount)
                        IsFull = true;
                    Dictionary<string, object> dict = new()
                        {
                            { nameof(IsFull), IsFull },
                            { nameof(PlayerNames), PlayerNames },
                        };
                    UpdateFields(OnCompleteAddPlayerName, dict);
                    PlayerIndicator = i;
                    addedName = true;
                }
        }

        // Gives resources for hexes matching the dice roll.
        public override void AllocateResources()
        {
            HexTile[] hexTiles = GameBoard.Hexes;
            for (int i = 0; i < hexTiles.Length; i++)
            {
                if (hexTiles[i].NumberToken == RollTotal && !hexTiles[i].HasRobber)
                {
                    VertexNode[] corners = hexTiles[i].Corners;
                    for (int k = 0; k < corners.Length; k++)
                        if (corners[k].PlayerIndex == PlayerIndicator)
                        {
                            int resourceAmount = corners[k].PieceType == BoardModel.PieceType.Town ? 1 : 2;
                            if (hexTiles[i].Terrain == BoardModel.TerrainType.Mountien)
                                PlayerOreCount += resourceAmount;
                            else if (hexTiles[i].Terrain == BoardModel.TerrainType.Hills)
                                PlayerBrickCount += resourceAmount;
                            else if (hexTiles[i].Terrain == BoardModel.TerrainType.Fields)
                                PlayerWheatCount += resourceAmount;
                            else if (hexTiles[i].Terrain == BoardModel.TerrainType.Pasture)
                                PlayerSheepCount += resourceAmount;
                            else if (hexTiles[i].Terrain == BoardModel.TerrainType.Forest)
                                PlayerWoodCount += resourceAmount;
                        }
                }
            }
        }

        // Gives starting resources for a new settlement.
        public override void AllocateStartingResources(int row, int column)
        {
            VertexNode vertexNode = GameBoard.Vertices[GameGrid.GetPieceLocationInArray(row, column)];
            for(int i = 0; i < GameBoard.Hexes.Length; i++)
            {
                for(int k = 0; k < GameBoard.Hexes[i].Corners.Length; k++)
                {
                    if(GameBoard.Hexes[i].Corners[k] == vertexNode)
                    {
                        if (GameBoard.Hexes[i].Terrain == BoardModel.TerrainType.Mountien)
                            PlayerOreCount++;
                        else if (GameBoard.Hexes[i].Terrain == BoardModel.TerrainType.Hills)
                            PlayerBrickCount++;
                        else if (GameBoard.Hexes[i].Terrain == BoardModel.TerrainType.Fields)
                            PlayerWheatCount++;
                        else if (GameBoard.Hexes[i].Terrain == BoardModel.TerrainType.Pasture)
                            PlayerSheepCount++;
                        else if (GameBoard.Hexes[i].Terrain == BoardModel.TerrainType.Forest)
                            PlayerWoodCount++;
                    }
                }
            }
        }   

        // Removes resources for a selected bank trade.
        public override void TradeWithBank(object parameter)
        {
            if (parameter is object[] data && data.Length == 2)
            {
                if (data[0] is String tradeType)
                {
                    if (data[1] is string resourceType)
                    {
                        TradeMessage = PlayerNames[PlayerIndicator] + Strings.EmptySpace + Strings.Traded;
                        int amountToGive = 0;
                        if (tradeType == Strings.FourToOne)
                            amountToGive = 4;
                        else if (tradeType == Strings.ThreeToOne)
                            amountToGive = 3;
                        else if (tradeType == Strings.TwoToOne)
                            amountToGive = 2;
                        TradeMessage += amountToGive + Strings.EmptySpace;
                        if (resourceType == Strings.WoodImage)
                        {
                            PlayerWoodCount -= amountToGive;
                            TradeMessage += Strings.WoodImage[..Strings.WoodImage.IndexOf('.')];
                        }
                        else if (resourceType == Strings.BrickImage)
                        {
                            PlayerBrickCount -= amountToGive;
                            TradeMessage += Strings.BrickImage[..Strings.BrickImage.IndexOf('.')];
                        }
                        else if (resourceType == Strings.SheepImage)
                        {
                            PlayerSheepCount -= amountToGive;
                            TradeMessage += Strings.SheepImage[..Strings.SheepImage.IndexOf('.')];
                        }
                        else if (resourceType == Strings.WheatImage)
                        {
                            PlayerWheatCount -= amountToGive;
                            TradeMessage += Strings.WheatImage[..Strings.WheatImage.IndexOf('.')];
                        }
                        else if (resourceType == Strings.OreImage)
                        {
                            PlayerOreCount -= amountToGive;
                            TradeMessage += Strings.OreImage[..Strings.OreImage.IndexOf('.')];
                        }
                    }
                }
            }
        }

        // Selects the resource card to receive from the bank.
        public override void PickCardToGet(object parameter)
        {
            if (parameter is ImageButton button)
            {
                SelectedTradeCard = button.Source.ToString()!;
                button.BorderWidth = 5;
                ResetSelctedCardBorder();
                PreviselySelctedCard = button;
            }
        }

        // Completes the selected bank trade.
        public override void ConfirmTradeWithBank()
        {
            TradeMessage += Strings.EmptySpace + Strings.For + 1 + Strings.EmptySpace;
            if (SelectedTradeCard.Contains(Strings.WoodImage))
            {
                PlayerWoodCount++;
                TradeMessage += Strings.WoodImage[..Strings.WoodImage.IndexOf('.')];
            }
            else if (SelectedTradeCard.Contains(Strings.BrickImage))
            {
                PlayerBrickCount++;
                TradeMessage += Strings.BrickImage[..Strings.BrickImage.IndexOf('.')];
            }
            else if (SelectedTradeCard.Contains(Strings.SheepImage))
            {
                PlayerSheepCount++;
                TradeMessage += Strings.SheepImage[..Strings.SheepImage.IndexOf('.')];
            }
            else if (SelectedTradeCard.Contains(Strings.WheatImage))
            {
                PlayerWheatCount++;
                TradeMessage += Strings.WheatImage[..Strings.WheatImage.IndexOf('.')];
            }
            else if (SelectedTradeCard.Contains(Strings.OreImage))
            {
                PlayerOreCount++;
                TradeMessage += Strings.OreImage[..Strings.OreImage.IndexOf('.')];
            }
            ResetSelctedCardBorder();
            ResourceCountersUpdated?.Invoke(this, EventArgs.Empty);
            Dictionary<string, object> dict = new()
                {
                    { nameof(TradeMessage), TradeMessage }
                };
            UpdateFields(dict);
            ShowTradeAlert();
        }

        // Sends a player-to-player trade request.
        public override void ConfirmTradeWithPlayer()
        {
            TradeInProgress = true;
            PlayersInTrade[0] = PlayerNames[PlayerIndicator];
            PlayersInTrade[1] = SelectedPlayerName;
            if (String.IsNullOrWhiteSpace(WoodTradeGiveAmount))
                WoodTradeGiveAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(BrickTradeGiveAmount))
                BrickTradeGiveAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(SheepTradeGiveAmount))
                SheepTradeGiveAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(WheatTradeGiveAmount))
                WheatTradeGiveAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(OreTradeGiveAmount))
                OreTradeGiveAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(WoodTradeGetAmount))
                WoodTradeGetAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(BrickTradeGetAmount))
                BrickTradeGetAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(SheepTradeGetAmount))
                SheepTradeGetAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(WheatTradeGetAmount))
                WheatTradeGetAmount = Strings.Zero;
            if (String.IsNullOrWhiteSpace(OreTradeGetAmount))
                OreTradeGetAmount = Strings.Zero;
            UpdateTradeParamaters();
        }

        // Cancels the active trade request.
        public override void CancelTradeRequest()
        {
            ResetTradeParameters();
            TradeMessage = Strings.TradeCanceled;
            UpdateTradeParamaters();
        }

        // Converts the current trade into a counter offer.
        public override void CounterOffer()
        {
            (PlayersInTrade[1], PlayersInTrade[0]) = (PlayersInTrade[0], PlayersInTrade[1]);
            (WoodTradeGiveAmount, WoodTradeGetAmount) = (WoodTradeGetAmount, WoodTradeGiveAmount);
            (BrickTradeGiveAmount, BrickTradeGetAmount) = (BrickTradeGetAmount, BrickTradeGiveAmount);
            (SheepTradeGiveAmount, SheepTradeGetAmount) = (SheepTradeGetAmount, SheepTradeGiveAmount);
            (WheatTradeGiveAmount, WheatTradeGetAmount) = (WheatTradeGetAmount, WheatTradeGiveAmount);
            (OreTradeGiveAmount, OreTradeGetAmount) = (OreTradeGetAmount, OreTradeGiveAmount);
            SelectedPlayerName = PlayersInTrade[1];
            TradeMessage = Strings.PlayerCounterOffer;
        }

        // Accepts the current trade offer.
        public override void AcceptTrade()
        {
            AllocateTradeResources();
            ResourceCountersUpdated?.Invoke(this, EventArgs.Empty);
            TradeMessage = Strings.TradeAccepted;
            UpdateTradeParamaters();
        }

        // Declines the current trade offer.
        public override void DeclineTrade()
        {
            TradeMessage = Strings.TradeDeclined;
            UpdateTradeParamaters();
        }

        // Checks whether a player trade can be sent.
        public override bool CenTradeWithPlayer()
        {
            bool givesACard =
                (!string.IsNullOrWhiteSpace(WoodTradeGiveAmount) && Convert.ToInt32(WoodTradeGiveAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(BrickTradeGiveAmount) && Convert.ToInt32(BrickTradeGiveAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(SheepTradeGiveAmount) && Convert.ToInt32(SheepTradeGiveAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(WheatTradeGiveAmount) && Convert.ToInt32(WheatTradeGiveAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(OreTradeGiveAmount) && Convert.ToInt32(OreTradeGiveAmount) > 0);
            bool getsACard =
                (!string.IsNullOrWhiteSpace(WoodTradeGetAmount) && Convert.ToInt32(WoodTradeGetAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(BrickTradeGetAmount) && Convert.ToInt32(BrickTradeGetAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(SheepTradeGetAmount) && Convert.ToInt32(SheepTradeGetAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(WheatTradeGetAmount) && Convert.ToInt32(WheatTradeGetAmount) > 0) ||
                (!string.IsNullOrWhiteSpace(OreTradeGetAmount) && Convert.ToInt32(OreTradeGetAmount) > 0);
            return givesACard && getsACard && !string.IsNullOrWhiteSpace(SelectedPlayerName);
        }

        // Checks whether this player has the resources to accept.
        public override bool CenAcceptTrade()
        {
            return (!string.IsNullOrWhiteSpace(WoodTradeGetAmount) && Convert.ToInt32(WoodTradeGetAmount) <= PlayerWoodCount) &&
                (!string.IsNullOrWhiteSpace(BrickTradeGetAmount) && Convert.ToInt32(BrickTradeGetAmount) <= PlayerBrickCount) &&
                (!string.IsNullOrWhiteSpace(SheepTradeGetAmount) && Convert.ToInt32(SheepTradeGetAmount) <= PlayerSheepCount) &&
                (!string.IsNullOrWhiteSpace(WheatTradeGetAmount) && Convert.ToInt32(WheatTradeGetAmount) <= PlayerWheatCount) &&
                (!string.IsNullOrWhiteSpace(OreTradeGetAmount) && Convert.ToInt32(OreTradeGetAmount) <= PlayerOreCount);
        }
        #endregion
    }
}
