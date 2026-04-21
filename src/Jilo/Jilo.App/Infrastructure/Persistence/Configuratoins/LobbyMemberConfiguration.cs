using Jilo.App.Domain.LobbyEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class LobbyMemberConfiguration : IEntityTypeConfiguration<LobbyMember>
{
    public void Configure(EntityTypeBuilder<LobbyMember> builder)
    {
        builder.HasKey(x => new { x.LobbyId, x.ProfileId });

        builder
            .HasOne(x => x.Profile)
            .WithMany(p => p.LobbyMemberships)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
