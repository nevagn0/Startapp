using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Domain.LobbyEntity;

public sealed class LobbyMember
{
    public Guid LobbyId { get; private init; }

    public Guid ProfileId { get; private init; }

    public DateTime JoinedAtUtc { get; private init; }

    public Lobby Lobby { get; private init; } = null!;

    public Profile Profile { get; private init; } = null!;

    public LobbyMember(Guid lobbyId, Guid profileId)
    {
        LobbyId = lobbyId;
        ProfileId = profileId;
        JoinedAtUtc = DateTime.UtcNow;
    }
}
