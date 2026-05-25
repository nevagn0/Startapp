using Jilo.App.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Username).IsUnique();

        builder.HasIndex(x => x.HasPremium);

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
