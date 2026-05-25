using ErrorOr;
using Jilo.App.Application.Features.Subscriptions.Subscribe;
using Jilo.App.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace Jilo.App.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/subscriptions/")]
public sealed class SubscriptionsController(IMediator mediator, IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost]
    public async Task<ActionResult> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        if (!HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshTokenValue))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new SubscribeCommand(profileId, refreshTokenValue);

        var result = await mediator.Send(command, cancellationToken);

        return result.MatchFirst<ActionResult>(
            onValue: value => {
                AddTokensToCookie(value.AccessToken, value.RefreshToken);
                return Ok();
                },
            onFirstError: error => error.Type switch
            {
                ErrorType.Unauthorized => Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Failure => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Forbidden => Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description)
            });
    }

    private void AddTokensToCookie(string accessToken, string refreshToken)
    {
        HttpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddSeconds(_jwtOptions.AccessTokenLifetimeSeconds)
        });

        HttpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(30)
        });
    }
}
