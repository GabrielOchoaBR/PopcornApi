using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.V1.Entities
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; }
        DateTime CreatedAt { get; }
        ObjectId? CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        ObjectId? UpdatedBy { get; set; }
    }
}