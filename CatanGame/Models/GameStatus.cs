namespace CatanGame.Models
{
    public class GameStatus
    {
        #region Enums
        public enum Status { Player1Turn, Player2Turn, Player3Turn, Player4Turn, Player5Turn, Player6Turn, YourTurn, PleseWait }
        #endregion

        #region Fields
        private readonly string[] msgs = [Strings.Player1Turn, Strings.Player2Turn, Strings.Player3Turn, Strings.Player4Turn, Strings.Player5Turn, Strings.Player6Turn, Strings.YourTurn, Strings.PleseWait];
        #endregion

        #region Properties
        public Status CurrentStatus { get; set; } = Status.PleseWait;
        public string StatusMessage => msgs[(int)CurrentStatus];
        #endregion
    }
}
