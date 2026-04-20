using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Profiles.UpdateAvatar;

public sealed record UpdateAvatarCommand(Guid ProfileId, string AvatarUrl) : ICommand;
