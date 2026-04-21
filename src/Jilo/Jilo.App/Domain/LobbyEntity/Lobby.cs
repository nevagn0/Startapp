using ErrorOr;
using Jilo.App.Domain.GameEntity;
using Jilo.App.Domain.RatingEntity;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Domain.LobbyEntity;

public sealed class Lobby
{
    public Guid Id { get; private init; }

    public Guid GameId { get; private init; }

    public Guid CreatedByProfileId { get; private init; }

    public DateTime CreatedAtUtc { get; private init; }

    public uint Version { get; private set; }

    public Game Game { get; private init; } = null!;

    public Profile CreatedByProfile { get; private init; } = null!;

    public ICollection<LobbyMember> Members { get; private set; }

    public bool IsActive { get; private set; }

    public ICollection<Rating> Ratings { get; private set; }

    public bool IsRatingAvailable => DateTime.UtcNow >= RatingAvailableSinceUtc && DateTime.UtcNow <= RatingAvailableUntilUtc;

    public DateTime? RatingAvailableSinceUtc { get; private set; }

    public DateTime? RatingAvailableUntilUtc { get; private set; }

    public Lobby(Guid gameId, Guid createdByProfileId)
    {
        Id = Guid.NewGuid();
        GameId = gameId;
        CreatedByProfileId = createdByProfileId;
        CreatedAtUtc = DateTime.UtcNow;
        Members = [];
        Ratings = [];
        IsActive = true;
    }

    public ErrorOr<Success> AddMember(Guid profileId)
    {
        if (Members.Any(lb => lb.ProfileId == profileId))
        {
            return Errors.Lobby.AlreadyInLobby;
        }

        Members.Add(new LobbyMember(Id, profileId));

        if (Members.Count >= 2)
        {
            RatingAvailableSinceUtc = DateTime.UtcNow;
            RatingAvailableUntilUtc = DateTime.UtcNow.AddHours(24);
        }

        return Result.Success;
    }

    public ErrorOr<Success> LeavingBy(Guid profileId)
    {
        var member = Members.FirstOrDefault(lb => lb.ProfileId == profileId);
        if (member is null)
        {
            return Errors.Lobby.NotAMemberOfLobby;
        }

        Members.Remove(member);

        return Result.Success;
    }
}
