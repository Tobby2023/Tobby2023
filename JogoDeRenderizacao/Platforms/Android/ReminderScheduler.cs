using Android.App;
using Android.Content;
using Android.OS;

namespace JogoDeRenderizacao
{
    public class ReminderScheduler
    {
        public void ScheduleReminder(long triggerAtMillis)
        {
            var context = Android.App.Application.Context;
            if (context == null)
            {
                throw new InvalidOperationException("Contexto não disponível.");
            }

            var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService)!;
            var intent = new Intent(context, typeof(AlarmReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.Immutable)!;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
            }
            else
            {
                alarmManager.SetExact(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
            }
        }
    }
}