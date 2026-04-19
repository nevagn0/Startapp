using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Profiles.Get;

public sealed record GetProfileQuery(Guid UserId) : IQuery<GetProfileQueryResponse>;
