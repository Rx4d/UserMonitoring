namespace Monitoring.Identity.Contracts;

public record class User(
    Guid Id,
    UserDetails Details,
    UserOnlineDetails OnlineDetails);