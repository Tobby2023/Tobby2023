namespace JogoDeRenderizacao.Components.Pages
{
    public partial class CanvaDraw
    {
        private int minutos { get; set; } = 1;
        private string hora { get; set; } = string.Empty;

        private void Agendar()
        {
            var dataDoAlarme = DateTime.Now.AddSeconds(30);
#if ANDROID
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S)
            {
                var alarmManager = (Android.App.AlarmManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.AlarmService)!;
                if (!alarmManager.CanScheduleExactAlarms())
                {
                    // Solicitar permissão para agendar alarmes exatos
                    var intent = new Android.Content.Intent(Android.Provider.Settings.ActionRequestScheduleExactAlarm);
                    Android.App.Application.Context.StartActivity(intent);
                    return;
                }
            }

            var reminderScheduler = new ReminderScheduler();
            var triggerTime = new DateTimeOffset(dataDoAlarme).ToUnixTimeMilliseconds();
            reminderScheduler.ScheduleReminder(triggerTime);
#endif
            hora = "Horário: " + dataDoAlarme.ToString("G");
        }
    }
}
