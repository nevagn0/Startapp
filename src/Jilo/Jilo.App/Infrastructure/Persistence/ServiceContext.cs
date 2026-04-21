using Jilo.App.Domain.GameEntity;
using Jilo.App.Domain.InvitationEntity;
using Jilo.App.Domain.LobbyEntity;
using Jilo.App.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence;

public sealed class ServiceContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Game> Games => Set<Game>();

    public DbSet<UserGame> UserGames => Set<UserGame>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<Lobby> Lobbies => Set<Lobby>();

    public DbSet<LobbyMember> LobbyMembers => Set<LobbyMember>();

    public DbSet<Invitation> Invitations => Set<Invitation>();

    public ServiceContext(DbContextOptions<ServiceContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
