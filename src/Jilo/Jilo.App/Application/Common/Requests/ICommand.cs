using ErrorOr;
using MediatR;

namespace Jilo.App.Application.Common.Requests;

public interface ICommand : IRequest<ErrorOr<Unit>>, ICommandBase;
