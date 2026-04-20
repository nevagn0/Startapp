
public record AddGameToUserRequest(
    Guid GameId,      
    string Role,
    string Rank
);

public record UpdateUserGameRequest(
    string Role,
    string Rank
);

public record UserGameResponse(
    Guid Id,
    Guid GameId,
    string GameName,
    string Description,
    string CoverImageUrl,
    string Role,
    string Rank,
    DateTime AddedAtUtc
);

public record AvailableGameResponse(
    Guid Id,
    string GameName,
    string Description,
    string CoverImageUrl
);