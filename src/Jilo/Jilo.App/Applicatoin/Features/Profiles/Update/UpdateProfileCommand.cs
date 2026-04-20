using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Profiles.Update;

public sealed record UpdateProfileCommand(Guid ProfileId, string? Bio) : ICommand<UpdateProfileCommandResponse>;
