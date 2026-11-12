using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace Monitoring.Client.Agent;

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

        ConfigureHideOnClose(builder);
        builder.Services.AddTransient<AgentRegistrationViewModel>();

        Action<HttpClient> configureClient = options => { options.BaseAddress = new("http://localhost:5130"); };
        builder.Services.AddHttpClient<AgentRegistrationService>(configureClient);
        builder.Services.AddHttpClient<AgentScreenshotUploaderService>(configureClient);

        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5130/hubs/users")
            .WithAutomaticReconnect()
            .Build();

        builder.Services.AddSingleton<AgentHeartbeatService>(provider => new(hubConnection, provider.GetRequiredService<AgentRegistrationService>()));
        builder.Services.AddSingleton<AgentScreenshotHubService>(provider => new(hubConnection, provider.GetRequiredService<AgentScreenshotUploaderService>()));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder ConfigureHideOnClose(MauiAppBuilder builder)
    {
        return builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(windows =>
            {
                windows.OnWindowCreated(window =>
                {
                    var mauiWindow = (MauiWinUIWindow)window;
                    var appWindow = mauiWindow.AppWindow;

                    appWindow.Closing += (_, args) =>
                    {
                        args.Cancel = true;
                        appWindow.Hide();
                    };
                });
            });
        });
    }
}