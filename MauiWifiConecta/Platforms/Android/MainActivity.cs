using Android.App;
using Android.Content.PM;
using Android.Net;
using Android.OS;

namespace MauiWifiConecta
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private static MainActivity Contexto;

        public MainActivity()
            => Contexto = this;

        public static string GetGatewayAddress()
        {
            var connectivityManager = (ConnectivityManager)Contexto.GetSystemService(ConnectivityService)!;
            var linkProperties = connectivityManager.GetLinkProperties(connectivityManager.ActiveNetwork);

            foreach (var route in linkProperties.Routes)
            {
                if (route.Destination.ToString() == "0.0.0.0/0")
                {
                    return route.Gateway!.HostAddress!;
                }
            }
            return "Gateway não encontrado";
        }
    }
}
