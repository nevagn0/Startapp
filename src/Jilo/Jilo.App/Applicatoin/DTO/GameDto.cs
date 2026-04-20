namespace Jilo.App.Applicatoin.DTO;

public sealed class GameDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string CoverImageUrl { get; init; }
}
