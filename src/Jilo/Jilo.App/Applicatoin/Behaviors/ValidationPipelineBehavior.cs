using ErrorOr;
using FluentValidation;
using Jilo.App.Applicatoin.Common.Requests;
using MediatR;

namespace Jilo.App.Applicatoin.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        Error[] validationErrors = validationResults
            .SelectMany(v => v.Errors)
            .Where(vf => vf != null)
            .Select(vf => Error.Validation(
                code: vf.PropertyName,
                description: vf.ErrorMessage))
            .Distinct()
            .ToArray();

        if (validationErrors.Length == 0)
        {
            return await next(cancellationToken);
        }

        return (dynamic)validationErrors;
    }
}
