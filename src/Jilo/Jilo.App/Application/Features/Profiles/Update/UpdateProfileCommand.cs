using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Profiles.Update;

public sealed record UpdateProfileCommand(Guid ProfileId, string? Bio) : ICommand<UpdateProfileCommandResponse>;
