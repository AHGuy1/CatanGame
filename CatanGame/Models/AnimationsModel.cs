namespace CatanGame.Models
{
    public abstract class AnimationsModel
    {
        public Color? TimeColor { get; protected set; }
        public double TimeOpacity { get; protected set; }
        public EventHandler? OpacityChanged;

        protected abstract void OnMessageReceived(long timeleft);
    }
}
