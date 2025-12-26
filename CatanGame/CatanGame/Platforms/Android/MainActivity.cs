using Android.App;
using Android.Content.PM;
using CommunityToolkit.Mvvm.Messaging;
using CatanGame.Models;
using Android.OS;

namespace CatanGame.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
