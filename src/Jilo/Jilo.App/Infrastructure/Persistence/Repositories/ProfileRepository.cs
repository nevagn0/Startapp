using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class ProfileRepository(ServiceContext context) : IProfileRepository
{
    public void Add(Profile profile)
    {
        context.Profiles.Add(profile);
    }

    public async Task<ErrorOr<Guid>> DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var profile = await context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
        {
            return Errors.Profile.NotFound;
        }

        context.Profiles.Remove(profile);

        return profile.Id;
    }

    public async Task<ErrorOr<Profile>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await context.Profiles.FindAsync([id], cancellationToken);

        if (profile is null)
        {
            return Errors.Profile.NotFound;
        }

        return profile;
    }

    public async Task<ErrorOr<Profile>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var profile = await context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
        {
            return Errors.Profile.NotFound;
        }

        return profile;
    }
}
