using System.Text.Json.Serialization;
using Domain.V1.Entities.Users;

namespace Application.V1.Dtos.Admin.Users
{
    public record UserGetDto(string Id,
                             string Name,
                             string Email,
                             string? RefreshToken,
                             DateTime? RefreshTokenCreatedAt,
                             DateTime? RefreshTokenExpiredAt,
                             IEnumerable<RoleType> Roles,
                             DateTime CreatedAt,
                             string? CreatedBy,
                             DateTime? UpdatedAt,
                             string? UpdatedBy)
    {
        [JsonIgnore]
        public string? RefreshToken { get; set; } = RefreshToken;

        [JsonIgnore]
        public DateTime? RefreshTokenCreatedAt { get; set; } = RefreshTokenCreatedAt;

        [JsonIgnore]
        public DateTime? RefreshTokenExpiredAt { get; set; } = RefreshTokenExpiredAt;
    }
}
