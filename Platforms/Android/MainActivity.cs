using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Lab1_Vardzar_2_4.Services;

namespace Lab1_Vardzar_2_4
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "studentapp",
        DataHost = "student")]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DeepLinkHandler.Handle(Intent?.Data?.ToString());
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            DeepLinkHandler.Handle(intent?.Data?.ToString());
        }
    }
}