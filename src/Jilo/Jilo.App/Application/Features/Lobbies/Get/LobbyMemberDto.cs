namespace Jilo.App.Application.Features.Lobbies.Get;

public sealed class LobbyMemberDto
{
    public required Guid Id { get; init; }

    public required string Username { get; init; }

    public required string Role { get; init; }

    public required string Rank { get; init; }

    public required bool IsOwner { get; init; }
}
