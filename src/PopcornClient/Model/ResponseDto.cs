namespace PopcornClient.Model
{
    public record ResponseDto<TDocument>(
        bool IsSuccess,
        TDocument? Response,
        FailedResult? FailedResult);
}
