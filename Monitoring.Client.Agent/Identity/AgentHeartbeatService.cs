using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Monitoring.Client.Agent;

public sealed class AgentHeartbeatService(HubConnection connection, AgentRegistrationService registrationService) : IAsyncDisposable
{
    private readonly TimeSpan _heartbeatInterval = TimeSpan.FromSeconds(5);

    private CancellationTokenSource? _cts;
    private Task? _worker;

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        await connection.DisposeAsync();
    }

    public void Start()
    {
        if (_worker is not null)
            return;

        _cts = new();
        _worker = ExecuteAsync(_cts.Token);
    }

    public async Task StopAsync()
    {
        if (_cts != null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
            _cts = null;
        }

        if (_worker != null)
        {
            await _worker;
            _worker = null;
        }

        await connection.StopAsync();
    }

    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync(cancellationToken);
                    await connection.InvokeAsync("JoinUserGroup", AgentIdentity.GetOrCreateId(), cancellationToken);
                }

                if (connection.State == HubConnectionState.Connected)
                {
                    await connection.InvokeAsync("Heartbeat", AgentIdentity.GetOrCreateId(), cancellationToken);
                }

                await Task.Delay(_heartbeatInterval, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (HubException ex) when (ex.Message.Contains("UserNotFound"))
            {
                await registrationService.RegisterAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Heartbeat error: {ex}");
            }
        }
    }
}