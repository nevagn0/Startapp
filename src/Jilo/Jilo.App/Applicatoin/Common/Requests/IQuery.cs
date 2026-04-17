using ErrorOr;
using MediatR;

namespace Jilo.App.Applicatoin.Common.Requests;

public interface IQuery<T> : IRequest<ErrorOr<T>>;
