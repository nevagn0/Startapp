using ErrorOr;
using Jilo.App.Domain;
using Jilo.App.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Jilo.App.Application.Features.Lobbies.Get;

public sealed class GetLobbyQueryHandler(
    ServiceContext context)
    : IRequestHandler<GetLobbyQuery, ErrorOr<GetLobbyQueryResponse>>
{
    public async Task<ErrorOr<GetLobbyQueryResponse>> Handle(GetLobbyQuery request, CancellationToken cancellationToken)
    {
        var lobby = await context.Lobbies
            .Where(l => l.Id == request.LobbyId)
            .Select(l => new GetLobbyQueryResponse
            {
                Id = l.Id,
                GameId = l.GameId,
                CreatedAtUtc = l.CreatedAtUtc,
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
