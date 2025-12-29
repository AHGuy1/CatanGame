using Android.App;
using Android.Content.PM;
using CommunityToolkit.Mvvm.Messaging;
using CatanGame.Models;
using Android.OS;
using Android.Content;

namespace CatanGame.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RegisterTimerMessages();
            StartDeleteFBDocsService();
        }

        private void StartDeleteFBDocsService()
        {
            Intent intent = new(this, typeof(DeleteFireBaseDocsService));
            StartService(intent);
        }

        private void RegisterTimerMessages()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<TimerSettings>>(this, (r, n) =>
            {
                OnMessageReceived(n.Value);
            });
        }

        private static void OnMessageReceived(TimerSettings value)
        {
            _ = new MyTimer(value.TotalTimeInMilliseconds, value.IntervalInMilliseconds).Start();
        }
    }
}
