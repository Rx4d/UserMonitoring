using Microsoft.AspNetCore.SignalR.Client;

namespace Monitoring.Client.Agent;

public sealed partial class AgentScreenshotHubService(HubConnection connection, AgentScreenshotUploaderService uploaderService) : IDisposable
{
    private IDisposable? _subscription;

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    public void Subscribe()
    {
        _subscription ??= connection.On<Guid>(
            "ScreenshotRequested",
            requestId => MainThread.InvokeOnMainThreadAsync(() => uploaderService.UploadAsync(requestId)));
    }
}