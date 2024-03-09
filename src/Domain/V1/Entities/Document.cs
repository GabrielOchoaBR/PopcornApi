using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.V1.Entities
{
    public abstract class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get => Id.CreationTime; }
        [BsonElement("createdBy")]
        public ObjectId? CreatedBy { get; set; }
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [BsonElement("updatedBy")]
        public ObjectId? UpdatedBy { get; set; }
    }
}
