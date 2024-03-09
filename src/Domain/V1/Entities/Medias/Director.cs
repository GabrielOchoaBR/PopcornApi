using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.V1.Entities.Medias
{
    public class Director
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public required string Name { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
