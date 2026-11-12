using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Monitoring.Client.Admin;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddTransient<UserTableViewModel>();

        Action<HttpClient> configureClient = options => { options.BaseAddress = new("http://localhost:5130"); };
        builder.Services.AddHttpClient<AdminApiService>(configureClient);

        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5130/hubs/users")
            .WithAutomaticReconnect()
            .Build();

        builder.Services.AddSingleton<UserScreenshotListenerService>(provider => new(hubConnection));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}