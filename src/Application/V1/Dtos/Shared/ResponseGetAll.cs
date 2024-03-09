namespace Application.V1.Dtos.Shared
{
    public record ResponseGetAll<T>(IEnumerable<T> Result, long TotalCount) where T : class;
}
