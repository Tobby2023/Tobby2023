using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification.EventArgs;

namespace MauiNotification.Service
{
    public static class LocalNotificationExtensions
    {
        public static ILocalNotificationBuilder ConfigurationNotification(this ILocalNotificationBuilder builder)
        {
            #region [Botões de ação na notificação]
            var entrarAction = new NotificationAction(1)
            {
                Title = "Entrar",
                Android =
                {
                    LaunchAppWhenTapped = true,
                    IconName = { ResourceName = "i0" }
                }
            };

            var fecharAction = new NotificationAction(2)
            {
                Title = "Fechar",
                Android =
                {
                    LaunchAppWhenTapped = false,
                    IconName = { ResourceName = "i1" }
                }
            };

            var cancelarAction = new NotificationAction(3)
            {
                Title = "Cancelar",
                Android =
                {
                    LaunchAppWhenTapped = true,
                    IconName = { ResourceName = "i2" }
                }
            };
            #endregion

            #region [Adicionando canal de notificação]
            builder.AddAndroid(android =>
            {

                android.AddChannel(new NotificationChannelRequest
                {
                    Id = "critical_channel",
                    Name = "Alarmes",
                    Importance = AndroidImportance.Max,
                    CanBypassDnd = true,
                    LockScreenVisibility = AndroidVisibilityType.Public
                });
            });
            #endregion

            #region [Adicionando Categorias]
            builder.AddCategory(new NotificationCategory(NotificationCategoryType.Reminder)
            {
                ActionList = new HashSet<NotificationAction>(new List<NotificationAction>
                {
                    entrarAction,
                    fecharAction,
                    cancelarAction
                })
            });
            #endregion

            return builder;
        }

        [Obsolete]
        public static void Current_NotificationActionTapped(NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {

            }
            else if (e.IsTapped)
            {

            }
            else
            {
                switch (e.ActionId)
                {
                    case 1:
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.Current!.MainPage!.DisplayAlert(e.Request.Title, e.Request.Subtitle, "OK");
                        });
                        break;

                    case 2:
                        LocalNotificationCenter.Current.Cancel(e.Request.NotificationId);
                        break;

                    case 3:
                        LocalNotificationCenter.Current.ClearAll();
                        break;
                }

            }
        }
    }
}