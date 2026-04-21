using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Profiles.Get;

public sealed record GetProfileQuery(Guid ProfileId) : IQuery<GetProfileQueryResponse>;
