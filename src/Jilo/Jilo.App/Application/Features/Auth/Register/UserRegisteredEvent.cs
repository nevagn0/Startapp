using MediatR;

namespace Jilo.App.Application.Features.Auth.Register;

public sealed record class UserRegisteredEvent(Guid UserId, string Username) : INotification;
