using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace JogoDeRenderizacao
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService)!;
            var channelId = "reminder_channel";
            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetSmallIcon(Resource.Drawable.ic_call_decline) // Substitua por um ícone válido
                .SetContentTitle("Lembrete")
                .SetContentText("Este é um lembrete: " + DateTime.Now.ToString("G"))
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetAutoCancel(true);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(channelId, "Reminders", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(channel);
            }

            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}