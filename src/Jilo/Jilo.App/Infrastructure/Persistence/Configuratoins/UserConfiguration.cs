using Jilo.App.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jilo.App.Infrastructure.Persistence.Configuratoins;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .HasConversion<string>();

        builder
            .HasMany(x => x.RefreshTokens)
            .WithOne(r => r.User)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Username).IsUnique();
    }
}
