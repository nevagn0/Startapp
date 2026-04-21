namespace Jilo.App.Application.Features.Profiles.Get;

public sealed class GameDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string CoverImageUrl { get; init; }

    public required string Rank { get; init; }

    public required string Role { get; init; }
}
