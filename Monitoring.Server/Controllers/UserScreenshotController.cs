using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Monitoring.Server.Controllers;

[ApiController]
[Route("users/screenshots")]
public sealed class UserScreenshotController(IHubContext<UserHub> userHub) : ControllerBase
{
    [HttpPost("{userId}/request")]
    public async Task<ActionResult<Guid>> RequestAsync(Guid userId, CancellationToken cancellationToken)
    {
        var requestId = Guid.NewGuid();

        await userHub.Clients
            .Group(UserHub.GetUserGroup(userId))
            .SendAsync("ScreenshotRequested", requestId, cancellationToken);

        return Accepted(requestId);
    }

    [HttpPost("{requestId}/upload")]
    [RequestSizeLimit(Limits.MaxPayloadSize)]
    public async Task<IActionResult> UploadAsync(Guid requestId, CancellationToken cancellationToken)
    {
        if (Request.ContentType != "image/jpeg")
            return BadRequest("Only JPEG images are supported.");

        await using var imageStream = new MemoryStream();
        await Request.Body.CopyToAsync(imageStream, cancellationToken);

        if (imageStream.Length == 0)
            return BadRequest("Empty screenshot.");

        await userHub.Clients
            .Group(UserHub.AdminGroup)
            .SendAsync("ScreenshotReceived", requestId, imageStream.ToArray(), cancellationToken);

        return Accepted();
    }
}