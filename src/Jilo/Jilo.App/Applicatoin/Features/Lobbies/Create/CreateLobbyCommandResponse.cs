namespace Jilo.App.Applicatoin.Features.Lobbies.Create;

public sealed class CreateLobbyCommandResponse
{
    public required Guid Id { get; init; }

    public required Guid GameId { get; init; }

    public required DateTime CreatedAtUtc { get; init; }
}
