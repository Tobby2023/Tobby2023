using Android.App;
using Android.Content;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Color = Android.Graphics.Color;

namespace AppNotification.Platforms.Android
{
    [BroadcastReceiver(Name = "com.companyname.appnotification.BootReceiver", Enabled = true, Exported = false, Permission = "android.permission.RECEIVE_BOOT_COMPLETED")]
    [IntentFilter(new[] {Intent.ActionBootCompleted, "android.intent.action.QUICKBOOT_POWERON"})]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            List<NotificationChannelRequest> notifications = new List<NotificationChannelRequest>()
            {
                new NotificationChannelRequest()
                {
                    Id = "geral",
                    Name = "Notificações Gerais",
                    Description = "Canal para notificações do app",
                    Importance = (AndroidImportance)NotificationImportance.High,
                    LockScreenVisibility = AndroidVisibilityType.Public,
                    CanBypassDnd = true,
                    EnableVibration = true,
                    EnableLights = true,
                    LightColor = new AndroidColor(Color.Red)
                    //Sound = ("content://settings/system/notification_sound")
                },
            };
            LocalNotificationCenter.CreateNotificationChannels(notifications); // Fallback

            // 2. Reagenda as notificações
            // Remove notificações antigas
            LocalNotificationCenter.Current.CancelAll();

            // Agenda novas
            for (int i = 100; i < 103; i++) // IDs altos para evitar conflitos
            {
                var request = new NotificationRequest
                {
                    NotificationId = i,
                    Title = "Título após reboot",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(i - 89)
                    }
                };
                LocalNotificationCenter.Current.Show(request);
            }
        }
    }
}
