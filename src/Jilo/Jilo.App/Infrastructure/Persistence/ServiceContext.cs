using Jilo.App.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence;

public sealed class ServiceContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public ServiceContext(DbContextOptions<ServiceContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
