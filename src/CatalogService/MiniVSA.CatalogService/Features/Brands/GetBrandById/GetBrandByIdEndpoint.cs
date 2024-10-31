using Carter;
using MediatR;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Features.Brands.GetBrandById
{
    public class GetBrandByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(pattern: "/Brands/{id:guid}",
                handler: async (Guid id, ISender sender) =>
                {
                    var query = new GetBrandByIdQuery(id);
                    var result = await sender.Send(query);
                    return result.FromResult();
                })
                .WithTags("Brands")
                .WithName("Get Brand By Id")
                .WithSummary("Get Brand By Id Flow")
                .WithDescription("You can get brand by id using this endpoint.")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
