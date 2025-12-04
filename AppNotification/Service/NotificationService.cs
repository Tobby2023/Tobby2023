using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace AppNotification.Service
{
    public class NotificationService
    {
        public async Task ScheduleNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var notification = new NotificationRequest
                {
                    NotificationId = new Random().Next(1000, 9999),
                    Title = title,
                    Description = message,
                    Android = new AndroidOptions { ChannelId = "geral" }
                };

                if (notifyTime != null)
                {
                    notification.Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = notifyTime,
                        // Para notificações recorrentes:
                        //RepeatType = NotificationRepeat.Daily,
                    };
                }

                await LocalNotificationCenter.Current.Show(notification);
            }
            else
            {
                await App.Current!.MainPage!.DisplayAlert(title, message, "OK");
            }
        }
    }
}
