using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Features.Brands.DeleteBrand
{
    public class DeleteBrandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(pattern: "/Brands/{id:guid}",
                handler: async ([FromRoute] Guid id, ISender sender) =>
                {
                    var command = new DeleteBrandCommand(id);
                    var result = await sender.Send(command);
                    return result.FromResult();
                })
                .WithTags("Brands")
                .WithName("Soft Delete Brand")
                .WithSummary("Soft Delete Brand Flow")
                .WithDescription("You can soft delete brand using this endpoint.")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
