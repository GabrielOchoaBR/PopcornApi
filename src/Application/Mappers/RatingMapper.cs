using Application.V1.Dtos.Medias.Rating;
using Domain.V1.Entities.Medias;
using MongoDB.Bson;

namespace Application.Mappers
{
    public static class RatingMapper
    {
        public static Rating ToDomainModel(this RatingPostDto dto) => new()
        {
            Id = ObjectId.GenerateNewId(),
            Name = dto.Name,
            AllowedAge = dto.AllowedAge
        };

        public static Rating ToDomainModel(this RatingGetDto dto) => new()
        {
            Id = new ObjectId(dto.Id),
            Name = dto.Name,
            AllowedAge = dto.AllowedAge
        };

        public static Rating? Parse(RatingPutDto? dto)
        {
            if (dto is null)
                return null;

            return new()
            {
                Id = ObjectId.GenerateNewId(),
                Name = dto.Name,
                AllowedAge = dto.AllowedAge
            };
        }

        public static RatingGetDto ToDto(this Rating rating) =>
            new(rating.Id.ToString(),
                rating.Name,
                rating.AllowedAge);
    }
}
