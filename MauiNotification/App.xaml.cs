namespace MauiNotification
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Verifica e solicita permissões necessárias
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                Task.Run(async () => {
                    await CheckAndRequestNotificationPermission();
                });
            }

            MainPage = new MainPage();
        }

        private async Task CheckAndRequestNotificationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.PostNotifications>();
            }
        }
    }
}
