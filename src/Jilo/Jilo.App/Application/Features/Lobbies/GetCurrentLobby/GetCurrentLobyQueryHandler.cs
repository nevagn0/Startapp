using ErrorOr;
using Jilo.App.Domain;
using Jilo.App.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Application.Features.Lobbies.GetCurrentLobby;

public sealed class GetCurrentLobyQueryHandler(
    ServiceContext context)
    : IRequestHandler<GetCurrentLobyQuery, ErrorOr<GetCurrentLobbyQueryResponse>>
{
    public async Task<ErrorOr<GetCurrentLobbyQueryResponse>> Handle(GetCurrentLobyQuery request, CancellationToken cancellationToken)
    {
        var lobby = await context.Lobbies
            .Where(l => l.IsActive && l.Members.Any(m => m.ProfileId == request.ProfileId && m.LeftAtUtc == null))
            .Select(l => new GetCurrentLobbyQueryResponse
            {
                Id = l.Id,
                GameId = l.GameId,
                CreatedAtUtc = l.CreatedAtUtc,
                IsRatingAvailable = l.IsRatingAvailable,
                IsActive = l.IsActive,
                Members = l.Members
                    .Select(m => new LobbyMemberDto
                    {
                        Id = m.Profile.Id,
                        AvatarUrl = m.Profile.AvatarUrl,
                        Rank = m.Profile.UserGames
                            .Where(ug => ug.GameId == l.GameId)
                            .Select(ug => ug.Rank)
                            .First(),
                        Role = m.Profile.UserGames
                            .Where(ug => ug.GameId == l.GameId)
                            .Select(ug => ug.Role)
                            .First(),
                        Username = m.Profile.Username,
                        IsOwner = l.CreatedByProfileId == m.ProfileId
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (lobby is null)
        {
            return Errors.Lobby.NotFound;
        }

        return lobby;
    }
}
