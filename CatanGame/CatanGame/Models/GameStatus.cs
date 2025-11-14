namespace CatanGame.Models
{
    public class GameStatus
    {
        private readonly string[] msgs = [Strings.Player1Turn, Strings.Player2Turn, Strings.Player3Turn, Strings.Player4Turn, Strings.Player5Turn, Strings.Player6Turn, Strings.YourTurn, Strings.PleseWait];
        public enum Status { Player1Turn, Player2Turn, Player3Turn, Player4Turn, Player5Turn, Player6Turn, YourTurn, PleseWait }
        public Status CurrentStatus { get; set; } = Status.Player1Turn;
        public string StatusMessage => msgs[(int)CurrentStatus];
    }
}
