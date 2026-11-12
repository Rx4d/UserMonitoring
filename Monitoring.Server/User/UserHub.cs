using Microsoft.AspNetCore.SignalR;

namespace Monitoring.Server;

public sealed class UserHub(UserService userRegistry) : Hub
{
    public const string AdminGroup = "admins";

    public static string GetUserGroup(Guid id)
    {
        return $"user:{id:N}";
    }

    public Task Heartbeat(Guid userId)
    {
        var ipAddress = Context.GetHttpContext()?.GetRemoteOrLocalIp();
        var success = userRegistry.Heartbeat(userId, ipAddress);

        if (!success)
            throw new HubException("UserNotFound");

        return Task.CompletedTask;
    }

    public Task JoinAdminGroup()
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup);
    }

    public Task JoinUserGroup(Guid userId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId));
    }
}