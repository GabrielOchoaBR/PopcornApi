using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.V1.Dtos.Shared
{
    public class QueryGetAll
    {
        public string SearchTerm { get; set; } = string.Empty;

        public string? SortColumn { get; set; }
        public string SortDirection { get; set; } = string.Empty;

        [BindRequired]
        public int PageIndex { get; set; }
        [BindRequired]
        public int PageSize { get; set; }
    }
}
