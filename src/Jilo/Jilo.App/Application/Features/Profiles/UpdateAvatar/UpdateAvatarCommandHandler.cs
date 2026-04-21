using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using MediatR;

namespace Jilo.App.Application.Features.Profiles.UpdateAvatar;

public sealed class UpdateAvatarCommandHandler(
    IProfileRepository repo)
    : IRequestHandler<UpdateAvatarCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var profile = await repo.GetAsync(request.ProfileId, cancellationToken);

        if (profile.IsError)
        {
            return profile.Errors;
        }

        profile.Value.UpdateAvatarUrl(request.AvatarUrl);

        return Unit.Value;
    }
}
