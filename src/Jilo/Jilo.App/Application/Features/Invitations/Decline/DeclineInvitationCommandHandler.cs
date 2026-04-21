using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain;
using MediatR;

namespace Jilo.App.Application.Features.Invitations.Decline;

public sealed class DeclineInvitationCommandHandler(
    IInvitationRepository invitationRepo)
    : IRequestHandler<DeclineInvitationCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeclineInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await invitationRepo.GetAsync(request.InvitationId, cancellationToken);
        if (invitation.IsError)
        {
            return invitation.Errors;
        }

        if (invitation.Value.ReceiverProfileId != request.RecieverProfileId)
        {
            return Errors.Invitation.NotAReciever;
        }

        var declineResult = invitation.Value.Decline();
        if (declineResult.IsError)
        {
            return declineResult.Errors;
        }

        return Unit.Value;
    }
}
