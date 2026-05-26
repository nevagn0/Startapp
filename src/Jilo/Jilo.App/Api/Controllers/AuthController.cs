using ErrorOr;
using Jilo.App.Api.Dto.Auth;
using Jilo.App.Application.Features.Auth.Login;
using Jilo.App.Application.Features.Auth.Logout;
using Jilo.App.Application.Features.Auth.Refresh;
using Jilo.App.Application.Features.Auth.Register;
using Jilo.App.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Jilo.App.Api.Controllers;

[Route("api/v1/auth/")]
[ApiController]
public sealed class AuthController(IMediator mediator, IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RegisterUserResponse>> RegisterAsync([FromBody]RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(request.Username, request.Password);

        var registerResult = await mediator.Send(command, cancellationToken);

        return registerResult.MatchFirst(
            onValue: value => Created(string.Empty, value),
            onFirstError: error => error.Type switch
            {
                ErrorType.Validation => ValidationProblem(new ValidationProblemDetails(
                    registerResult.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()))),
                ErrorType.Conflict => Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: $"Unexpected error. {error.Description}")
            });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> LoginAsync([FromBody] LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(request.Username, request.Password);

        var loginResult = await mediator.Send(command, cancellationToken);

        return loginResult.MatchFirst<ActionResult>(
            onValue: value =>
            {
                AddTokensToCookie(value.AccessToken, value.RefreshToken);
                return Ok();
            },
            onFirstError: error => error.Type switch
            {
                ErrorType.Unauthorized => Problem(statusCode: StatusCodes.Status401Unauthorized),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description)
            });
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RefreshAsync(CancellationToken cancellationToken = default)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshTokenValue))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new RefreshCommand(refreshTokenValue);

        var refreshResult = await mediator.Send(command, cancellationToken);

        return refreshResult.MatchFirst<ActionResult>(
            onValue: value =>
            {
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
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. {error.Description}")
            });
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> LogoutAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
        {
            var command = new LogoutCommand(refreshToken);

            await mediator.Send(command, cancellationToken);
        }

        RemoveTokensFromCookie();

        return Ok();
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

    private void RemoveTokensFromCookie()
    {
        HttpContext.Response.Cookies.Delete("access_token");
        HttpContext.Response.Cookies.Delete("refresh_token");
    }
}
