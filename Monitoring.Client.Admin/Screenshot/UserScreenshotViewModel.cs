namespace Monitoring.Client.Admin;

public sealed class UserScreenshotViewModel
{
    public required Guid RequestId { get; init; }
    public required ImageSource Image { get; init; }
}