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
        });

        return new Window(new AppShell());
    }
}