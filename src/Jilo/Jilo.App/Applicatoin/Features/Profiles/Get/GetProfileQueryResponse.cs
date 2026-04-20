using Jilo.App.Domain.GameEntity;

namespace Jilo.App.Applicatoin.Features.Profiles.Get;

public sealed record GetProfileQueryResponse
{
    public required Guid Id { get; init; }
    
    public required string Username { get; init; }
    
    public required string? Bio { get; init; }
    
    public required string? AvatarUrl { get; init; }

    public required List<GameDto> Games { get; init; }
}
