using ErrorOr;
using Jilo.App.Domain;
using Jilo.App.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Application.Features.Profiles.Get;

public sealed class GetProfileQueryHandler(
    ServiceContext context)
    : IRequestHandler<GetProfileQuery, ErrorOr<GetProfileQueryResponse>>
{
    public async Task<ErrorOr<GetProfileQueryResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await context.Profiles
            .Where(p => p.Id == request.ProfileId)
            .Select(p => new GetProfileQueryResponse
            {
                Id = p.Id,
                Username = p.Username,
                Bio = p.Bio,
                AvatarUrl = p.AvatarUrl,
                Rating = p.Rating,
                Games = p.UserGames
                    .Select(ug => new GameDto()
                    {
                        Id = ug.GameId,
                        Name = ug.GameCatalog.GameName,
                        CoverImageUrl = ug.GameCatalog.CoverImageUrl,
                        Rank = ug.Rank,
                        Role = ug.Role
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            return Errors.Profile.NotFound;
        }

        return profile;
    }
}
