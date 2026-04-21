using ErrorOr;
using Jilo.App.Application.Common.Requests;
using Jilo.App.Infrastructure.Persistence;
using MediatR;

namespace Jilo.App.Application.Behaviors;

public sealed class TransactionalPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
    where TResponse : IErrorOr
{
    private readonly ServiceContext _context;

    public TransactionalPipelineBehavior(ServiceContext context) => _context = context;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await next(cancellationToken);

            if (result is IErrorOr { IsError: false })
            {
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            return result;
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            return (dynamic)Error.Failure(
                code: "Database.UpdateFail",
                description: $"Failed to update database. Details: {e.Message}");
        }
    }
}
