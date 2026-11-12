namespace Monitoring.Client.Admin;

public sealed partial class App : Application
{
    private readonly UserScreenshotListenerService _screenshotListenerService;

    public App(UserScreenshotListenerService screenshotListenerService)
    {
        InitializeComponent();
        _screenshotListenerService = screenshotListenerService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Dispatcher.DispatchAsync(_screenshotListenerService.Subscribe);
        return new Window(new AppShell());
    }
}