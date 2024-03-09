using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.V1.Entities.Medias
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? AllowedAge { get; set; }
    }
}
