using Application.V1.Dtos.Medias.Director;
using Application.V1.Dtos.Medias.Rating;
using Domain.V1.Entities.Medias;

namespace Application.V1.Dtos.Medias
{
    public record MediaPostDto(ContentType ContentType,
                               string Title,
                               string? Description,
                               DateTime? DateAdded,
                               int? ReleaseYear,
                               int? Duration,
                               RatingPostDto Rating,
                               DirectorPostDto Director,
                               IEnumerable<string> Cast,
                               IEnumerable<string>? Countries);
}
