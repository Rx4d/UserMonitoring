namespace Monitoring.Identity.Contracts;

public record class UserOnlineDetails(
    string? IpAddress,
    DateTimeOffset FirstSeenAt,
    DateTimeOffset LastHeartbeatAt);