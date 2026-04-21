namespace Jilo.App.Api.Dto.Ratings;

public sealed record RatingRequest(Guid TargetProfileId, RatingValue Value);
