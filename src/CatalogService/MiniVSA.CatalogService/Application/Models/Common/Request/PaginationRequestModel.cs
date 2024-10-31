using Microsoft.AspNetCore.Mvc;

namespace MiniVSA.CatalogService.Application.Models.Common.Request
{
    public class PaginationRequestModel
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
