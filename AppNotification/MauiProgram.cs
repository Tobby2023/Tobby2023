using AppNotification.Service;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using Plugin.LocalNotification;

namespace AppNotification
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration = new SnackbarConfiguration()
                {
                    PositionClass = Defaults.Classes.Position.BottomCenter
                };
            });
            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<IPService>();
            builder.Services.AddSingleton<ConectDevice>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
# endif

            return builder.Build();
        }
    }
}
