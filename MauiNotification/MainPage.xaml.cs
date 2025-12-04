using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace MauiNotification
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
            LocalNotificationCenter.Current.NotificationReceived += Current_NotificationReceived;
        }

        private void Current_NotificationReceived(NotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Current_NotificationActionTapped(NotificationActionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
