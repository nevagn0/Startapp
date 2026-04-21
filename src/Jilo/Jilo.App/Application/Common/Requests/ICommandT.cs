using ErrorOr;
using MediatR;

namespace Jilo.App.Application.Common.Requests;

public interface ICommand<T> : IRequest<ErrorOr<T>>, ICommandBase;
