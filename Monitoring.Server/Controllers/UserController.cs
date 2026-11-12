using Microsoft.AspNetCore.Mvc;
using Monitoring.Identity.Contracts;
using System.Collections.Immutable;

namespace Monitoring.Server.Controllers;

[ApiController]
[Route("users")]
public sealed class UserController(UserService userService) : ControllerBase
{
    [HttpGet]
    public ActionResult<ImmutableArray<User>> Get()
    {
        return Ok(userService.Get());
    }

    [HttpPost]
    public IActionResult Create([FromBody] UserCreation data)
    {
        userService.Create(data, HttpContext.GetRemoteOrLocalIp());

        return Created();
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteAsync(Guid userId)
    {
        userService.Delete(userId);

        return NoContent();
    }
}