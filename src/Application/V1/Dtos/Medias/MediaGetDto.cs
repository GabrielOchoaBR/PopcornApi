using Application.V1.Dtos.Medias.Director;
using Application.V1.Dtos.Medias.Rating;
using Domain.V1.Entities.Medias;

namespace Application.V1.Dtos.Medias
{
    public record MediaGetDto(string Id,
                              string Title,
                              ContentType? ContentType,
                              string? Description,
                              DateTime? DateAdded,
                              int? ReleaseYear,
                              int? Duration,
                              RatingGetDto? Rating,
                              DirectorGetDto? Director,
                              IEnumerable<string>? Cast,
                              IEnumerable<string>? Countries,
                              DateTime CreatedAt,
                              string? CreatedBy,
                              DateTime? UpdatedAt,
                              string? UpdatedBy)
    {
        public MediaGetDto() :
            this(string.Empty,
                 string.Empty,
                 null,
                 string.Empty,
                 null,
                 0,
                 0,
                 null,
                 null,
                 null,
                 null,
                 DateTime.MinValue,
                 string.Empty,
                 null,
                 string.Empty)
        {
        }
    }
}
