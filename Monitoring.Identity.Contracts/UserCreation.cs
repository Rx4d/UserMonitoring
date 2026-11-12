namespace Monitoring.Identity.Contracts;

public record class UserCreation(
    Guid UserId,
    UserDetails UserDetails);