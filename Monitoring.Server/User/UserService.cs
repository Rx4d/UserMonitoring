using Microsoft.Extensions.Caching.Memory;
using Monitoring.Identity.Contracts;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Monitoring.Server;

public sealed class UserService(IMemoryCache memoryCache)
{
    private const string CacheKey = "Monitoring.Identity.Users";
    private readonly ConcurrentDictionary<Guid, User> _users = memoryCache.GetOrCreate(CacheKey, cache => new ConcurrentDictionary<Guid, User>()) ?? new();

    public ImmutableArray<User> Get()
    {
        return [.. _users.Values];
    }

    public bool Create(UserCreation data, string? ipAddress)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.UserId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.");

        var now = DateTimeOffset.UtcNow;
        var user = new User(data.UserId, data.UserDetails, new(ipAddress, now, now));

        return _users.TryAdd(user.Id, user);
    }

    public bool Delete(Guid userId)
    {
        return _users.TryRemove(userId, out _);
    }

    public bool Heartbeat(Guid userId, string? ipAddress)
    {
        if (_users.TryGetValue(userId, out var currentUser))
        {
            var updatedUser = currentUser with
            {
                OnlineDetails = currentUser.OnlineDetails with
                {
                    IpAddress = ipAddress ?? currentUser.OnlineDetails.IpAddress,
                    LastHeartbeatAt = DateTimeOffset.UtcNow
                }
            };

            return _users.TryUpdate(userId, updatedUser, currentUser);
        }

        return false;
    }
}