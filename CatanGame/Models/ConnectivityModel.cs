namespace CatanGame.Models
{
    public abstract class ConnectivityModel
    {
        #region Fields
        protected bool IsConnectedPri { get; set; }
        #endregion

        #region Properties
        public abstract bool IsConnected {get; protected set; }
        public EventHandler? ConnectivityChanged { get; set; }
        #endregion

        #region Constructor
        protected abstract void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e);
        #endregion
    }
}
