using System.Text.Json.Serialization;

namespace Domain.V1.Entities.Users
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        Read,
        Write,
    }
}
