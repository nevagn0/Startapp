using Jilo.App.Domain.GameEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
{
    public void Configure(EntityTypeBuilder<UserGame> builder)
    {

        builder.HasKey(ug => ug.Id);

        builder.HasOne(ug => ug.Profile)
              .WithMany(p => p.UserGames)
              .HasForeignKey(ug => ug.ProfileId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ug => ug.GameCatalog)
            .WithMany(g => g.UserGames);

        builder.HasIndex(ug => new { ug.ProfileId, ug.GameId })
            .IsUnique();
    }
}