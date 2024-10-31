using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniVSA.CatalogService.Application.Models.Common;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Features.Brands.GetAllBrands
{
    public class GetAllBrandsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(pattern: "/Brands",
                handler: async (ISender sender, int pageNumber = 1, int pageSize = 10) =>
                {
                    var query = new GetAllBrandsQuery(new PaginationRequestModel { PageNumber = pageNumber, PageSize = pageSize });
                    var result = await sender.Send(query);
                    return result.FromResult();
                })
                .WithName("Get All Brands")
                .WithTags("Brands")
                .WithSummary("Get All Brands Flow")
                .WithDescription("You can get all brands using this endpoint.")
                .Produces(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
