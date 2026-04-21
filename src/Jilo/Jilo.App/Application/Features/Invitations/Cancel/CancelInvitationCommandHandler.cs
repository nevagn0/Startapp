using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain;
using MediatR;

namespace Jilo.App.Application.Features.Invitations.Cancel;

public sealed class CancelInvitationCommandHandler(
    IInvitationRepository repo)
    : IRequestHandler<CancelInvitationCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CancelInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await repo.GetAsync(request.InvitationId, cancellationToken);
        if (invitation.IsError)
        {
            return invitation.Errors;
        }

        if (invitation.Value.SenderProfileId != request.SenderProfileId)
        {
            return Errors.Invitation.NotAnOwner;
        }

        var cancelResult = invitation.Value.Cancel();
        if(cancelResult.IsError)
        {
            return cancelResult.Errors;
        }

        return Unit.Value;
    }
}
