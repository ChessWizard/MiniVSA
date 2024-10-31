using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniVSA.CatalogService.Application.Models.Common.Request;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Features.Brands.UpdateBrand
{
    public record UpdateBrandRequest(string? Name,
                                     FileUploadRequestModel? Image);

    public class UpdateBrandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch(pattern: "/Brands/{id:guid}",
                handler: async ([FromRoute] Guid id, [FromBody] UpdateBrandRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateBrandCommand>()
                                         .Build(id);
                    var result = await sender.Send(command);
                    return result.FromResult();
                })
                .WithTags("Brands")
                .WithName("Update Brand as Partially")
                .WithSummary("Update Brand as Partially Flow")
                .WithDescription("You can update brand as partially using this endpoint.")
                .DisableAntiforgery()
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
