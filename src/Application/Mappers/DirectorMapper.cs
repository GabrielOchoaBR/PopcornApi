using Application.V1.Dtos.Medias.Director;
using Domain.V1.Entities.Medias;
using MongoDB.Bson;

namespace Application.Mappers
{
    public static class DirectorMapper
    {
        public static Director ToDomainModel(this DirectorPostDto dto) => new()
        {
            Id = ObjectId.GenerateNewId(),
            Name = dto.Name,
            BirthDate = dto.BirthDate
        };

        public static Director ToDomainModel(this DirectorGetDto dto) => new()
        {
            Id = new ObjectId(dto.Id),
            Name = dto.Name,
            BirthDate = dto.BirthDate
        };

        public static Director? Parse(DirectorPutDto? dto)
        {
            if (dto is null)
                return null;

            return new()
            {
                Id = ObjectId.GenerateNewId(),
                Name = dto.Name,
                BirthDate = dto.BirthDate
            };
        }

        public static DirectorGetDto ToDto(this Director Director) =>
            new(Director.Id.ToString(),
                Director.Name,
                Director.BirthDate);
    }
}
