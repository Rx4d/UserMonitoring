using Microsoft.AspNetCore.SignalR.Client;

namespace Monitoring.Client.Admin;

public sealed class UserScreenshotListenerService(HubConnection connection)
{
    private IDisposable? _subscription;

    public void Dispose()
    {
        _subscription?.Dispose();
    }

    public async Task Subscribe()
    {
        if (_subscription is not null) return;

        _subscription = connection.On<Guid, byte[]>(
            "ScreenshotReceived",
            (requestId, image) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _ = ShowScreenshotPageAsync(new()
                    {
                        RequestId = requestId,
                        Image = ImageSource.FromStream(() => new MemoryStream(image))
                    });
                });
            });

        await connection.StartAsync();
        await connection.InvokeAsync("JoinAdminGroup");
    }

    private static Task ShowScreenshotPageAsync(UserScreenshotViewModel viewModel)
    {
        return Shell.Current.CurrentPage.Navigation.PushAsync(new UserScreenshotPage(viewModel));
    }
}