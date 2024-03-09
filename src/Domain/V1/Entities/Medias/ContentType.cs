using System.Text.Json.Serialization;

namespace Domain.V1.Entities.Medias
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContentType
    {
        Movie,
        TVShow
    }
}
