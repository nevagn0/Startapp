using Jilo.App.Api.Authorization.Requirements;
using Jilo.App.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Jilo.App.Api.Authorization.Handlers;

public sealed class LobbyOwnerRequirementHandler(
    ServiceContext dbContext,
    IHttpContextAccessor httpContextAccessor) 
    : AuthorizationHandler<LobbyOwnerRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LobbyOwnerRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return;
        }

        if(!httpContext.Request.RouteValues.TryGetValue("id", out var lobbyIdRoute)
            || !Guid.TryParse(lobbyIdRoute?.ToString(), out var lobbyId))
        {
            return;
        }

        var profileIdStr = context.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;

        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return;
        }

        bool exists = await dbContext.Lobbies
            .AnyAsync(l => l.Id == lobbyId && l.CreatedByProfileId == profileId && l.IsActive);

        if (exists)
        {
            context.Succeed(requirement);
        }
    }
}
