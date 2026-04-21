using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Profiles.UpdateAvatar;

public sealed record UpdateAvatarCommand(Guid ProfileId, string AvatarUrl) : ICommand;
