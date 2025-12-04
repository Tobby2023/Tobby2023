using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace AppNotification
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
        protected override void OnStart()
        {
            base.OnStart();
#if ANDROID
            List<NotificationChannelRequest> notifications = new List<NotificationChannelRequest>()
            {
                new NotificationChannelRequest()
                {
                    Id = "geral",
                    Name = "Notificações Gerais",
                    Description = "Canal para notificações do app",
                    Importance = (AndroidImportance)Android.App.NotificationImportance.High,
                    LockScreenVisibility = AndroidVisibilityType.Public,
                    CanBypassDnd = true,
                    EnableVibration = true,
                    EnableLights = true,
                    LightColor = new AndroidColor(Android.Graphics.Color.Red)
                },
            };

            LocalNotificationCenter.CreateNotificationChannels(notifications); // Fallback
#endif

        }
    }
}
