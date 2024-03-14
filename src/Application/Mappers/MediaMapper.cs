using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Medias.Director;
using Application.V1.Dtos.Medias.Rating;
using Domain.V1.Entities.Medias;

namespace Application.Mappers
{
    public static class MediaMapper
    {
        public static Media ToDomainModel(this MediaPostDto dto) => new()
        {
            Title = dto.Title,
            ContentType = dto.ContentType,
            Description = dto.Description,
            DateAdded = dto.DateAdded,
            ReleaseYear = dto.ReleaseYear,
            Duration = dto.Duration,
            Rating = dto.Rating?.ToDomainModel(),
            Director = dto.Director?.ToDomainModel(),
            Cast = dto.Cast,
            Countries = dto.Countries,
        };

        public static Media ToDomainModel(this MediaGetDto mediaGetDto) => new()
        {
            Id = new MongoDB.Bson.ObjectId(mediaGetDto.Id),
            Title = mediaGetDto.Title,
            ContentType = mediaGetDto.ContentType,
            Description = mediaGetDto.Description,
            DateAdded = mediaGetDto.DateAdded,
            ReleaseYear = mediaGetDto.ReleaseYear,
            Duration = mediaGetDto.Duration,
            Rating = mediaGetDto.Rating?.ToDomainModel(),
            Director = mediaGetDto.Director?.ToDomainModel(),
            Cast = mediaGetDto.Cast,
            Countries = mediaGetDto.Countries,
            CreatedBy = mediaGetDto.CreatedBy != null ? new MongoDB.Bson.ObjectId(mediaGetDto.CreatedBy) : null,
            UpdatedAt = mediaGetDto.UpdatedAt,
            UpdatedBy = mediaGetDto.UpdatedBy != null ? new MongoDB.Bson.ObjectId(mediaGetDto.UpdatedBy) : null
        };

        public static void Parse(this Media media, MediaPutDto dto)
        {
            media.Title = dto.Title;
            media.ContentType = dto.ContentType;
            media.Description = dto.Description;
            media.DateAdded = dto.DateAdded;
            media.ReleaseYear = dto.ReleaseYear;
            media.Duration = dto.Duration;

            media.Rating = RatingMapper.Parse(dto.Rating);
            media.Director = DirectorMapper.Parse(dto.Director);

            media.Cast = dto.Cast;
            media.Countries = dto.Countries;
        }

        public static MediaGetDto ToDto(this Media media) =>
            new(media.Id.ToString(),
                media.Title,
                media.ContentType,
                media.Description,
                media.DateAdded,
                media.ReleaseYear,
                media.Duration,
                media.Rating?.ToDto(),
                media.Director?.ToDto(),
                media.Cast,
                media.Countries,
                media.CreatedAt,
                media.CreatedBy?.ToString(),
                media.UpdatedAt,
                media.UpdatedBy?.ToString());

        public static IQueryable<MediaGetDto> SelectToDto(this IQueryable<Media> medias) =>
            medias.Select(x => new MediaGetDto(x.Id.ToString(),
                                            x.Title,
                                            x.ContentType,
                                            x.Description,
                                            x.DateAdded,
                                            x.ReleaseYear,
                                            x.Duration,
                                            x.Rating != null ?
                                                            new RatingGetDto(x.Rating.Id.ToString(), x.Rating.Name, x.Rating.AllowedAge)
                                                        : null,
                                            x.Director != null ?
                                                            new DirectorGetDto(x.Director.Id.ToString(), x.Director.Name, x.Director.BirthDate)
                                                        : null,
                                            x.Cast,
                                            x.Countries,
                                            x.CreatedAt,
                                            x.CreatedBy.ToString(),
                                            x.UpdatedAt,
                                            x.UpdatedBy.ToString()));
    }
}
