using Jilo.App.Api.Authorization.Requirements;
using Jilo.App.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jilo.App.Api.Authorization.Handlers;

public sealed class ProfileOwnerRequirementHandler(
    ServiceContext dbContext,
    IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<ProfileOwnerRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileOwnerRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return;
        }

        var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
        {
            return;
        }

        if (await dbContext.Profiles.AnyAsync(p => p.UserId == userId))
        {
            context.Succeed(requirement);
        }
    }
}
