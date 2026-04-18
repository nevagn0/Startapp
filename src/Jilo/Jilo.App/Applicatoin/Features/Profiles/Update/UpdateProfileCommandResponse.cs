namespace Jilo.App.Applicatoin.Features.Profiles.Update;

public sealed record UpdateProfileCommandResponse(Guid Id, Guid UserId, string Username, string? Bio, string? AvatarUrl);
