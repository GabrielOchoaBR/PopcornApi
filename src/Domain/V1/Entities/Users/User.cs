using Domain.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.V1.Entities.Users
{
    [BsonCollection("User")]
    public class User : Document
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenCreatedAt { get; set; }
        public DateTime? RefreshTokenExpiredAt { get; set; }


        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public IEnumerable<RoleType> Roles { get; set; } = [];
    }
}
