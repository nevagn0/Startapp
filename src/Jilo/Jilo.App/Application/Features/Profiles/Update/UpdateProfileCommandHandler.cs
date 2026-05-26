using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using MediatR;

namespace Jilo.App.Application.Features.Profiles.Update;

public sealed class UpdateProfileCommandHandler(
    IProfileRepository repo)
    : IRequestHandler<UpdateProfileCommand, ErrorOr<UpdateProfileCommandResponse>>
{
    public async Task<ErrorOr<UpdateProfileCommandResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await repo.GetAsync(request.ProfileId, cancellationToken);

        if (profile.IsError)
        {
            return profile.Errors;
        }

        if (request.Bio is not null)
        {
            profile.Value.UpdateBio(request.Bio);
        }

        return new UpdateProfileCommandResponse(
            profile.Value.Id,
            profile.Value.UserId,
            profile.Value.Username,
            profile.Value.Bio);
    }
}
