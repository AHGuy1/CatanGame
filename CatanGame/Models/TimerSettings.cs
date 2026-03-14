namespace CatanGame.Models
{
    public class TimerSettings(long totalTimeInMilliseconds, long intervalInMilliseconds)
    {
        #region Properties
        public long TotalTimeInMilliseconds { get; set; } = totalTimeInMilliseconds;
        public long IntervalInMilliseconds { get; set; } = intervalInMilliseconds;
        #endregion
    }
}
