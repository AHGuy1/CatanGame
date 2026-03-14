namespace CatanGame.Models
{
    public abstract class AnimationsModel
    {
        #region Events
        public EventHandler? OpacityChanged;
        #endregion

        #region Properties
        public Color? TimeColor { get; protected set; }
        public double TimeOpacity { get; protected set; }
        #endregion

        #region PrivateMethods
        protected abstract void OnMessageReceived(long timeleft);
        #endregion
    }
}
