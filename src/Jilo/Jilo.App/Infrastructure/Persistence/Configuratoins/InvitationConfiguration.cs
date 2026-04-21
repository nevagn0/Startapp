using Jilo.App.Domain.InvitationEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<string>();

        builder
            .HasOne(x => x.SenderProfile)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.ReceiverProfile)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Lobby)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ExpiresAtUtc);
        builder.HasIndex(x => x.LobbyId);
        builder.HasIndex(x => new { x.ReceiverProfileId, x.Status });
    }
}
