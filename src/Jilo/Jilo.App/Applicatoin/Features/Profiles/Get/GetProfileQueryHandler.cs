using ErrorOr;
using Jilo.App.Domain;
using Jilo.App.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Applicatoin.Features.Profiles.Get;

public sealed class GetProfileQueryHandler(
    ServiceContext context)
    : IRequestHandler<GetProfileQuery, ErrorOr<GetProfileQueryResponse>>
{
    public async Task<ErrorOr<GetProfileQueryResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await context.Profiles
            .Where(p => p.UserId == request.UserId)
            .Select(p => new GetProfileQueryResponse
            {
                Id = p.Id,
                Username = p.Username,
                Bio = p.Bio,
                AvatarUrl = p.AvatarUrl
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            return Errors.Profile.NotFound;
        }

        return profile;
    }
}
