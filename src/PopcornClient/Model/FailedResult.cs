using PopcornApi.Model.WebApi;

namespace PopcornClient.Model
{
    public record FailedResult(string Title, int StatusCode, ExceptionResponse ExceptionResponse)
    {
    }
}
