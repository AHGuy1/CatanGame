using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class Connectivity : ConnectivityModel
    {
        #region Properties
        public override bool IsConnected
        {
            get => IsConnectedPri;
            protected set
            {
                if (IsConnectedPri != value)
                {
                    IsConnectedPri = value;
                    ConnectivityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Constructor
        public Connectivity()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            IsConnected = Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }
        #endregion

        #region Private Methods
        protected override void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
        }
        #endregion
    }
}
