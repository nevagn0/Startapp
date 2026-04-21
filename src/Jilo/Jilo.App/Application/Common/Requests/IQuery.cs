using ErrorOr;
using MediatR;

namespace Jilo.App.Application.Common.Requests;

public interface IQuery<T> : IRequest<ErrorOr<T>>;
