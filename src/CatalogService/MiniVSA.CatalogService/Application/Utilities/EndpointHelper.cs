using MiniVSA.CatalogService.Application.Models.Result;

namespace MiniVSA.CatalogService.Application.Utilities
{
    public static class EndpointHelper
    {
        public static IResult FromResult<T>(this BaseResult<T> result)
            => Results.Json(result, statusCode: result.HttpStatusCode);
    }
}
