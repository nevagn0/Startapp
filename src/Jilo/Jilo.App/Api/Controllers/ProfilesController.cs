using ErrorOr;
using Jilo.App.Api.Authorization;
using Jilo.App.Api.Dto.Profiles;
using Jilo.App.Applicatoin.Features.Profiles.Get;
using Jilo.App.Applicatoin.Features.Profiles.Update;
using Jilo.App.Applicatoin.Features.Profiles.UpdateAvatar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jilo.App.Api.Controllers;

[ApiController]
[Route("api/v1/profile/")]
public sealed class ProfilesController(
    IMediator mediator,
    IWebHostEnvironment env) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetProfileQueryResponse>> GetMyAsync(CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var query = new GetProfileQuery(userId);

        var profile = await mediator.Send(query, cancellationToken);

        return profile.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetProfileQueryResponse>> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = new GetProfileQuery(userId);

        var profile = await mediator.Send(query, cancellationToken);

        return profile.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [Authorize(Policy = PolicyNames.ProfileOwner)]
    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UpdateProfileCommandResponse>> UpdateAsync([FromBody]UpdateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new UpdateProfileCommand(userId, request.Bio);

        var updateResult = await mediator.Send(command, cancellationToken);

        return updateResult.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: error.Code,
                    detail: $"Possibly you're trying to change another person's profile. Detail: {error.Description}"),
                ErrorType.Validation => ValidationProblem(new ValidationProblemDetails(
                    updateResult.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()))),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [Authorize]
    [HttpPost("me/avatar")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadAvatar(IFormFile file, CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
            return BadRequest("Only JPG, PNG, WEBP images are allowed.");

        if (file.Length > 5 * 1024 * 1024) // 5 MB
            return BadRequest("Max file size is 5 MB.");

        var fileName = $"{userId}_{Guid.NewGuid()}{ext}";
        var uploadsFolder = Path.Combine(env.WebRootPath, "avatars");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var avatarUrl = $"{Request.Scheme}://{Request.Host}/avatars/{fileName}";

        var command = new UpdateAvatarCommand(userId, avatarUrl);
        var updateAvatarResult = await mediator.Send(command, cancellationToken);
        
        return updateAvatarResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }
}
