namespace Application.V1.Dtos.Medias.Rating
{
    public record RatingGetDto(string? Id, string Name, int? AllowedAge)
    {
        public RatingGetDto() : this(null, string.Empty, 0)
        {
        }
    }
}
