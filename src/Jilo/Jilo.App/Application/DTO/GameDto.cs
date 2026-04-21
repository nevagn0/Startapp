namespace Jilo.App.Application.DTO;

public sealed class GameDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string CoverImageUrl { get; init; }
    
    public required List<string> Roles { get; init; }
    
    public required List<string> Ranks { get; init; }
}
