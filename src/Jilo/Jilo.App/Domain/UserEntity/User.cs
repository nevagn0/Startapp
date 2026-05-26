namespace Jilo.App.Domain.UserEntity;

public sealed class User
{
    public Guid Id { get; private init; }

    public string Username { get; private set; }

    public string PasswordHash { get; private set; }

    public Role Role { get; private set; }

    public DateTime CreatedAtUtc { get; private init; }

    public DateTime UpdatedAtUtc { get; private set; }

    public Profile Profile { get; private set; } = null!;

    public ICollection<RefreshToken> RefreshTokens { get; private set; }

    public User(string username, string passwordHash, Role role)
    {
        Id = Guid.NewGuid();
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        RefreshTokens = [];
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
