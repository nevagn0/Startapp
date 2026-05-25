using ErrorOr;
using Jilo.App.Domain.GameEntity;
using Jilo.App.Domain.LobbyEntity;

namespace Jilo.App.Domain.UserEntity;

public sealed class Profile
{
    public Guid Id { get; private init; }

    public Guid UserId { get; private init; }

    public string Username { get; private set; }

    public string? Bio { get; private set; }

    public string? AvatarUrl { get; private set; }

    public int Rating { get; private set; }

    public User User { get; private init; } = null!;

    public bool HasPremium { get; private set; }

    public ICollection<UserGame> UserGames { get; private set; }

    public ICollection<Lobby> CreatedLobbies { get; private set; }

    public ICollection<LobbyMember> LobbyMemberships { get; private set; }

    public Profile(Guid userId, string username, string? bio = null, string? avatarUrl = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Username = username;
        Bio = bio;
        Rating = 0;
        AvatarUrl = avatarUrl;
        UserGames = [];
        CreatedLobbies = [];
        LobbyMemberships = [];
        HasPremium = false;
    }

    public void UpdateRating(int delta)
    {
        Rating += delta;
    }

    public void UpdateBio(string bio)
    {
        Bio = bio;
    }

    public void UpdateAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
    }

    public ErrorOr<Success> Subscribe()
    {
        if (HasPremium)
        {
            return Errors.User.AlreadyPremiumUser;
        }

        HasPremium = true;
        return Result.Success;
    }
}
