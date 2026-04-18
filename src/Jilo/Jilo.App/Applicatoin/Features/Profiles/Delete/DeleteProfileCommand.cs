using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Profiles.Delete;

public sealed record DeleteProfileCommand(Guid UserId) : ICommand;
