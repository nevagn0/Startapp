using ErrorOr;
using MediatR;

namespace Jilo.App.Applicatoin.Common.Requests;

public interface ICommand<T> : IRequest<ErrorOr<T>>, ICommandBase;
