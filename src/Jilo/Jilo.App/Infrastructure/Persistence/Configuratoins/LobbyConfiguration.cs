using Jilo.App.Domain.LobbyEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class LobbyConfiguration : IEntityTypeConfiguration<Lobby>
{
    public void Configure(EntityTypeBuilder<Lobby> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Version)
            .IsRowVersion();

        builder
            .HasOne(x => x.Game)
            .WithMany(g => g.Lobbies)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.CreatedByProfile)
            .WithMany(p => p.CreatedLobbies)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Members)
            .WithOne(m => m.Lobby)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.GameId);
        builder.HasIndex(x => x.CreatedByProfileId);
        builder.HasIndex(x => new { x.GameId, x.IsActive });
    }
}
