using Jilo.App.Domain.RatingEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => new { r.LobbyId, r.RaterProfileId, r.TargetProfileId })
               .IsUnique();

        builder.Property(r => r.Value).IsRequired();

        builder.HasOne(r => r.Lobby)
               .WithMany(l => l.Ratings)
               .HasForeignKey(r => r.LobbyId)
               .OnDelete(DeleteBehavior.Cascade); // если лобби удаляется, оценки тоже

        builder.HasOne(r => r.RaterProfile)
               .WithMany()
               .HasForeignKey(r => r.RaterProfileId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.TargetProfile)
               .WithMany()
               .HasForeignKey(r => r.TargetProfileId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
