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
        public override GameStatus Status => _status;
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
        private static Color GetStatusColor(int playerTurn)
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

        protected override void IntArrayBoardPieces()
        {
            for (int i = 0; i < 276; i++)
                BoardPieces[i] = string.Empty;
        }

        protected override void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) => OnMessageReceived(m.Value));
        }

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

        protected override void StartTimer()
        {
            TimerSettings ts = new((TurnTime * 1000) + 1, 10);
            WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(ts));
        }

        protected override void StopTimer()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<string>(Keys.StopSignal));
        }

        protected override void OnCompletePlayerLeft(Task task)
        {
            PlayerLeft?.Invoke(this, PlayerIndicator);
        }

        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                for (int i = 1; i < PlayerCount; i++)
                    if (!String.IsNullOrWhiteSpace(PlayerNames[i]) && String.IsNullOrWhiteSpace(updatedGame.PlayerNames[i]))
                    {
                        for (int j = 1; j < PlayerCount; j++)
                            if (PlayerNames[j] != updatedGame.PlayerNames[j])
                            {
                                PlayerLeft?.Invoke(this, j);
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
                    WinnerIndecator = updatedGame.WinnerIndecator;
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.Navigation.PushAsync(new EndGamePage(this), true);
                    });
                }
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
                if (MonoplizingPlayer != updatedGame.MonoplizingPlayer && updatedGame.MonoplizingPlayer != string.Empty)
                {
                    MonoplizingPlayer = updatedGame.MonoplizingPlayer;
                    MonopolizedCard = updatedGame.MonopolizedCard;
                    PlayersPassed++;
                    if (MonopolizedCard == Strings.WoodImage)
                    {
                        MonoplizedCardsCount += PlayerWoodCount;
                        PlayerWoodCount = 0;
                    }
                    else if (MonopolizedCard == Strings.BrickImage)
                    {
                        MonoplizedCardsCount += PlayerBrickCount;
                        PlayerBrickCount = 0;
                    }
                    else if (MonopolizedCard == Strings.SheepImage)
                    {
                        MonoplizedCardsCount += PlayerSheepCount;
                        PlayerSheepCount = 0;
                    }
                    else if (MonopolizedCard == Strings.WheatImage)
                    {
                        MonoplizedCardsCount += PlayerWheatCount;
                        PlayerWheatCount = 0;
                    }
                    else if (MonopolizedCard == Strings.OreImage)
                    {
                        MonoplizedCardsCount += PlayerOreCount;
                        PlayerOreCount = 0;
                    }
                    Dictionary<string, object> dict = new()
                        {
                            { nameof(PlayersPassed), PlayersPassed },
                            { nameof(MonoplizedCardsCount), MonoplizedCardsCount }
                        };
                    UpdateFields(dict);
                    gridChanged = true;
                }
                if (PlayersPassed == PlayerCount && MonoplizingPlayer == PlayerNames[PlayerIndicator])
                {
                    if (MonopolizedCard == Strings.WoodImage)
                        PlayerWoodCount += MonoplizedCardsCount;
                    else if (MonopolizedCard == Strings.BrickImage)
                        PlayerBrickCount = MonoplizedCardsCount;
                    else if (MonopolizedCard == Strings.SheepImage)
                        PlayerSheepCount = MonoplizedCardsCount;
                    else if (MonopolizedCard == Strings.WheatImage)
                        PlayerWheatCount = MonoplizedCardsCount;
                    else if (MonopolizedCard == Strings.OreImage)
                        PlayerOreCount = MonoplizedCardsCount;
                    MonoplizingPlayer = string.Empty;
                    MonopolizedCard = string.Empty;
                    MonoplizedCardsCount = 0;
                    PlayersPassed = 0;
                    Dictionary<string, object> dict = new()
                        {
                            { nameof(PlayersPassed), PlayersPassed },
                            { nameof(PlayersPassed), PlayersPassed },
                            { nameof(MonopolizedCard), MonopolizedCard },
                            { nameof(MonoplizingPlayer), MonoplizingPlayer }
                        };
                    UpdateFields(dict);
                    gridChanged = true;
                }
                if (Turn != updatedGame.Turn)
                {
                    Rold = false;
                    PlayerTurn = updatedGame.PlayerTurn;
                    Turn = updatedGame.Turn;
                    TurnChanged?.Invoke(this, EventArgs.Empty);
                    gridChanged = true;
                    StartTimer();
                }
                for (int i = 0; i < BoardPieces.Length; i++)
                    if (BoardPieces[i] != updatedGame.BoardPieces[i])
                    {
                        gridChanged = true;
                        BoardPieces[i] = updatedGame.BoardPieces[i];
                    }
                if (LongestRoadLength != updatedGame.LongestRoadLength || LargestArmySize != updatedGame.LargestArmySize)
                {
                    LongestRoadLength = updatedGame.LongestRoadLength;
                    LargestArmySize = updatedGame.LargestArmySize;
                    gridChanged = true;
                }
                if (RobberPlacment[0] != updatedGame.RobberPlacment[0] || RobberPlacment[1] != updatedGame.RobberPlacment[1])
                {
                    RobberPlacment = updatedGame.RobberPlacment;
                    gridChanged = true;
                }
                if (IsRolling != updatedGame.IsRolling && !Rold)
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
            else
            {
                if (!GameStarted)
                    GameDeleted?.Invoke(this, Strings.HostLeft);
                else
                    GameDeleted?.Invoke(this, string.Empty);
            }
        }

        protected override void ShowTradeAlert()
        {
            MainThread.InvokeOnMainThreadAsync(() => Toast.Make(TradeMessage, ToastDuration.Long, 15).Show());
        }

        protected override void OnCompleteDeleted(Task task)
        {
            if (task.IsCompletedSuccessfully)
                GameDeleted?.Invoke(this, string.Empty);
        }

        protected override void OnCompleteAddPlayerName(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameEror, ToastDuration.Long, 14);
        }

        protected override void OnTurnChanged(Task task)
        {
            if (task.IsCompletedSuccessfully)
                GameChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void UpdateStatus()
        {
            Array status = Enum.GetValues(typeof(GameStatus.Status));
            _status.CurrentStatus = !GameStarted ? GameStatus.Status.PleseWait :
                PlayerTurn == PlayerIndicator + 1 ? GameStatus.Status.YourTurn :
                (GameStatus.Status)status.GetValue(PlayerTurn - 1)!;
            StatusColor = GetStatusColor(PlayerTurn);
        }

        protected override void ResetSelctedCardBorder()
        {
            if (PreviselySelctedCard != null)
                PreviselySelctedCard.BorderWidth = 0;
        }

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

        protected override void RecivedTrade()
        {
            TradeRecived?.Invoke(this, EventArgs.Empty);
        }

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

        public override void CloseTrade()
        {
            CloseTradePopUp?.Invoke(this, EventArgs.Empty);
        }

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

        protected override void ReciveCounterOffer()
        {
            MainThread.InvokeOnMainThreadAsync(() => Toast.Make(TradeMessage, ToastDuration.Long, 20).Show());
            RecivedTrade();
        }
        #endregion

        #region Public Methods
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

        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        public override void UpdateFields(Action<Task> OnComplete, Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override void UpdateFields(Dictionary<string, object> dict)
        {
            fbd.UpdateFields(Keys.GamesCollection, Id, dict);
        }

        public override void GetDocument(string Id, Action<IDocumentSnapshot> OnComplete)
        {
            fbd.GetDocument(Keys.GamesCollection, Id, OnComplete);
        }

        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        public override void RemoveSnapshotListener()
        {
            StopTimer();
            ilr?.Remove();
            PlayerNames[PlayerIndicator] = string.Empty;
            if (PlayerIndicator == 0 || GameStarted)
                DeleteDocument(OnCompleteDeleted);
            else
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

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id);
            fbd.DeleteDocument(Keys.GameCodesCollection, GameCode, OnComplete);
        }

        public override void EndTurn()
        {
            if (PlayerTurn == PlayerCount)
                PlayerTurn = 1;
            else
                PlayerTurn++;
            Turn++;
            Rold = false;
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerTurn), PlayerTurn },
                { nameof(Turn), Turn }
            };
            if (PlayerPoints >= PointsGoal)
            {
                WinnerIndecator = PlayerIndicator;
                dict.Add(nameof(WinnerIndecator), WinnerIndecator);
            }
            UpdateFields(OnTurnChanged, dict);
            GridChanged?.Invoke(this, EventArgs.Empty);
            StartTimer();
        }

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

        public override void CancelTradeRequest()
        {
            ResetTradeParameters();
            TradeMessage = Strings.TradeCanceled;
            UpdateTradeParamaters();
        }

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

        public override void AcceptTrade()
        {
            AllocateTradeResources();
            ResourceCountersUpdated?.Invoke(this, EventArgs.Empty);
            TradeMessage = Strings.TradeAccepted;
            UpdateTradeParamaters();
        }

        public override void DeclineTrade()
        {
            TradeMessage = Strings.TradeDeclined;
            UpdateTradeParamaters();
        }

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
