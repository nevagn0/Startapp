using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Domain.GameEntity;

public sealed class UserGame
{
    public Guid Id { get; private init; }
    public Guid ProfileId { get; private set; }
    public Guid GameId { get; private set; }
    public string Role { get; private set; }
    public string Rank { get; private set; }
    public DateTime AddedAtUtc { get; private init; }

    public Game GameCatalog { get; private set; } = null!;
    public Profile Profile { get; private set; } = null!; 

    public UserGame(Guid profileId, Guid gameId, string role, string rank)
    {
        Id = Guid.NewGuid();
        ProfileId = profileId;
        GameId = gameId;
        Role = role;
        Rank = rank;
        AddedAtUtc = DateTime.UtcNow;
    }
    public void Update(string role, string rank)
    {
        Role = role;
        Rank = rank;
    }
}