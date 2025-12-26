using Android.OS;
using CatanGame.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace CatanGame.Platforms.Android
{
    public class MyTimer: CountDownTimer
    { 
        public MyTimer(long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval)
        {
            WeakReferenceMessenger.Default.Register<AppMessage<string>>(this, (r, n) =>
            {
                OnMessageReceived(n.Value);
            });
        }

        private void OnMessageReceived(string value)
        {
            if (value != Keys.StopSignal)
            {
                Cancel();
            }
        }

        public override void OnFinish()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(Keys.FinishedSignal));
        }
        public override void OnTick(long millisUntilFinished)
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(millisUntilFinished));

        }
    }
}
