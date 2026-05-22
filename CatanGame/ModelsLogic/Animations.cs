using CatanGame.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace CatanGame.ModelsLogic
{
    public class Animations : AnimationsModel
    {
        #region Constructor
        public Animations()
        {
            TimeColor = Colors.Black;
            TimeOpacity = 1.0;

            MainThread.InvokeOnMainThreadAsync(() =>
            {
                WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) => OnMessageReceived(m.Value));
            });
        }
        #endregion

        #region Private Methods
        // Updates timer color and opacity based on remaining time.
        protected override void OnMessageReceived(long timeleft)
        {
            if (timeleft == Keys.FinishedSignal)
            {
                TimeOpacity = 1.0;
                TimeColor = Colors.Black;
                OpacityChanged?.Invoke(this, EventArgs.Empty);
            }
            else if ((double)timeleft / 1000 <= 10.0)
            {
                TimeColor = new Color(232, 28, 45);

                if ((((double)timeleft / 1000) % 1) >= 0.5)
                    TimeOpacity = (((double)timeleft / 1000) % 0.5) * 2;
                else
                    TimeOpacity = 1 - (((double)timeleft / 1000) % 0.5) * 2;

                OpacityChanged?.Invoke(this, EventArgs.Empty);
            }
            else if(TimeOpacity != 1 || TimeColor != Colors.Black)
            {
                TimeOpacity = 1.0;
                TimeColor = Colors.Black;
                OpacityChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
