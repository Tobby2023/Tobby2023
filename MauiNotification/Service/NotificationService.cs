using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace MauiNotification.Service
{
    public class NotificationService
    {
        public async Task ScheduleNotification(DateTime date)
        {
            // Agendando uma notificação que aparecerá mesmo com app fechado
            var request = new NotificationRequest
            {
                NotificationId = date.Second,
                Title = "Lembrete: " + date.ToString(),
                Description = "Hora da sua tarefa! " + date.ToString(),
                CategoryType = NotificationCategoryType.Reminder,
                Android = new AndroidOptions
                {
                    ChannelId = "critical_channel",
                    Priority = AndroidPriority.Max,
                    AutoCancel = true,
                    TimeoutAfter = TimeSpan.FromHours(24),
                    VibrationPattern = [200, 300, 200, 300]

                },
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = date,
                    Android = new AndroidScheduleOptions
                    {
                        AllowedDelay = TimeSpan.FromMinutes(1),
                        AlarmType = AndroidAlarmType.RtcWakeup
                    }
                }
            };

            await LocalNotificationCenter.Current.Show(request);
        }
    }
}
