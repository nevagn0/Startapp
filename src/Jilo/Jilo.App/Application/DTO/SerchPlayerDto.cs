namespace Jilo.App.Application.DTO;

public record SearchPlayersRequest(
    Guid GameId,
    string? Rank,
    string? Role,
    string? Query         
);

public record PlayerSearchResponse(
    Guid ProfileId,
    string Username,
    string Bio,
    string? AvatarUrl,
    int Rating,
    string GameName,
    string Role,
    string Rank,
    DateTime AddedAtUtc
);