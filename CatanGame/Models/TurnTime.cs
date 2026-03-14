namespace CatanGame.Models
{
    public class TurnTime
    {
        #region Properties
        public int Time { get; set; }
        public string Minutes => Time >= 60 ? $"{Time / 60}" + Strings.EmptySpace + Strings.MinutesLabel : string.Empty;
        public string Seconds => Time % 60 != 0 ? $"{Time % 60}" + Strings.EmptySpace + Strings.SecondsLabel : string.Empty;
        public string DisplayName => Seconds != string.Empty && Minutes != string.Empty ? Minutes + Strings.EmptySpace + Seconds : Seconds != string.Empty ? Seconds : Minutes;
        #endregion

        #region Constructor
        public TurnTime(int time)
        {
            Time = time;
        }

        public TurnTime()
        {
            Time = 45;
        }
        #endregion
    }
}
