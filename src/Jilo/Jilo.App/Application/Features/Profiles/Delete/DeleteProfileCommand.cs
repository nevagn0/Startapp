using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Profiles.Delete;

public sealed record DeleteProfileCommand(Guid UserId) : ICommand;
