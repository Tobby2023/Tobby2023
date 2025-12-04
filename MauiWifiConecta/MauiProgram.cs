using CommunityToolkit.Maui;
using MauiWifiConecta.Service;
using Microsoft.Extensions.Logging;

namespace MauiWifiConecta
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            // Registro dos serviços
            //builder.Services.AddSingleton<SocketServerService>();
            //builder.Services.AddSingleton<ClienteService>();
            //builder.Services.AddSingleton<IPSercice>();

            builder.Services.AddSingleton<HostConectado>();
            builder.Services.AddSingleton<ConectorSocket>();
            builder.Services.AddSingleton<IPSercice>();
            builder.Services.AddSingleton<WebSocketServerService>();
            builder.Services.AddSingleton<WebSocketClientService>();

            return builder.Build();
        }
    }
}
