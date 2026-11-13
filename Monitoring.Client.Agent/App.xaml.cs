using Microsoft.Win32;

namespace Monitoring.Client.Agent;

public sealed partial class App : Application
{
    private readonly AgentHeartbeatService _heartbeatService;
    private readonly AgentScreenshotHubService _screenshotHubService;

    public App(AgentHeartbeatService heartbeatService, AgentScreenshotHubService screenshotHubService)
    {
        InitializeComponent();

        _heartbeatService = heartbeatService;
        _screenshotHubService = screenshotHubService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Dispatcher.Dispatch(() =>
        {
            _heartbeatService.Start();
            _screenshotHubService.Subscribe();
            SetStartup(true);
        });

        return new Window(new AppShell());
    }

    private static void SetStartup(bool enable)
    {
        const string appName = "Monitoring.Client.Agent";
        const string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(runKey, true);
            if (key is null) return;

            if (enable && !string.IsNullOrEmpty(Environment.ProcessPath))
            {
                key.SetValue(appName, $"\"{Environment.ProcessPath}\"");
            }
            else
            {
                key.DeleteValue(appName, throwOnMissingValue: false);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }
}