using MongoDB.Bson;

namespace Application.V1.Dtos.Medias.Rating
{
    public record RatingPostDto(string Name, int? AllowedAge);
}
