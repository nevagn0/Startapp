using Microsoft.AspNetCore.Mvc;

namespace Jilo.App.Api.Controllers;

[ApiController]
[Route("api/v1/auth/")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(CancellationToken cancellationToken = default)
    {
        return Created();
    }

}
