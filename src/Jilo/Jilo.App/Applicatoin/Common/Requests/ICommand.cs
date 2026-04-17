using ErrorOr;
using MediatR;

namespace Jilo.App.Applicatoin.Common.Requests;

public interface ICommand : IRequest<ErrorOr<Unit>>, ICommandBase;
