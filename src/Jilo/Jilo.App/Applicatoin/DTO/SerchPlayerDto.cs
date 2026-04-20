namespace Jilo.App.Applicatoin.DTO;

public record SearchPlayersRequest(
    Guid GameId,
    string? Rank,
    string? Role,
    string? Query         
);

public record PlayerSearchResponse(
    Guid UserId,
    string Username,
    string Bio,
    string? AvatarUrl,
    int Rating,
    string GameName,
    string Role,
    string Rank,
    DateTime AddedAtUtc
);