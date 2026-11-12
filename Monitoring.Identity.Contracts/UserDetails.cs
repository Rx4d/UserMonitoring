namespace Monitoring.Identity.Contracts;

public record class UserDetails(
    string Username,
    string DomainName,
    string MachineName);