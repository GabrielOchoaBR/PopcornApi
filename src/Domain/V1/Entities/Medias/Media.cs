using Domain.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.V1.Entities.Medias
{
    [BsonCollection("Media")]
    public class Media : Document
    {
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public ContentType? ContentType { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? ReleaseYear { get; set; }
        public int? Duration { get; set; }
        public Rating? Rating { get; set; }
        public Director? Director { get; set; }
        public IEnumerable<string>? Cast { get; set; } = [];
        public IEnumerable<string>? Countries { get; set; } = [];
    }
}
