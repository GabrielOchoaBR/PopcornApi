using MongoDB.Bson;

namespace Application.V1.Dtos.Medias.Rating
{
    public record RatingPutDto(string Name, int? AllowedAge);
}
