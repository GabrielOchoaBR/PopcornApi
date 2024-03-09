namespace PopcornApi.Model.WebApi
{
    public record ExceptionResponse(string Title,
                                    string Details,
                                    int StatusCode,
                                    IDictionary<string, string[]>? Erros)
    {
    }
}
