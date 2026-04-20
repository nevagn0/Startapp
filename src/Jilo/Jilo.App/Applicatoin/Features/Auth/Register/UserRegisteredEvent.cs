using MediatR;

namespace Jilo.App.Applicatoin.Features.Auth.Register;

public sealed record class UserRegisteredEvent(Guid UserId, string Username) : INotification;
