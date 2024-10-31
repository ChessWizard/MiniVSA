namespace MiniVSA.CatalogService.Application.Models.Result.Paging
{
    public class PagingMetaData(int pageSize, int currentPage, int totalCount)
    {
        public int PageSize { get; set; } = pageSize;

        public int CurrentPage { get; set; } = currentPage;

        public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);

        public int TotalCount { get; set; } = totalCount;

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;
    }
}
