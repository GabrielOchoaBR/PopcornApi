using Application.V1.Dtos.Medias.Director;
using Application.V1.Dtos.Medias.Rating;
using Domain.V1.Entities.Medias;

namespace Application.V1.Dtos.Medias
{
    public record MediaPutDto(string Id,
                              string Title,
                              ContentType? ContentType,
                              string? Description,
                              DateTime? DateAdded,
                              int? ReleaseYear,
                              int? Duration,
                              RatingPutDto? Rating,
                              DirectorPutDto? Director,
                              IEnumerable<string>? Cast,
                              IEnumerable<string>? Countries);
}
