using Microsoft.AspNetCore.Authorization;

namespace Jilo.App.Api.Authorization.Requirements;

public sealed record LobbyOwnerRequirement : IAuthorizationRequirement;
