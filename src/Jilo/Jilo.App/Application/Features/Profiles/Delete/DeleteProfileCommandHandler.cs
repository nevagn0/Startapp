using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using MediatR;

namespace Jilo.App.Application.Features.Profiles.Delete;

public sealed class DeleteProfileCommandHandler(
    IProfileRepository repo)
    : IRequestHandler<DeleteProfileCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await repo.DeleteByUserIdAsync(request.UserId, cancellationToken);

        if (deleteResult.IsError)
        {
            return deleteResult.Errors;
        }

        return Unit.Value;
    }
}
