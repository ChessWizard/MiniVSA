using Carter;
using Mapster;
using MediatR;
using MiniVSA.CatalogService.Application.Models.Common.Request;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Features.Brands.CreateBrand
{
    public record CreateBrandRequest(string Name,
                                      FileUploadRequestModel Image);
    public class CreateBrandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(pattern: "/Brands",
                handler: async (CreateBrandRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateBrandCommand>();
                    var result = await sender.Send(command);
                    return result.FromResult();
                })
                .WithName("Create Brand")
                .WithTags("Brands")
                .WithSummary("Create Brand Flow")
                .WithDescription("You can create brand using this endpoint.")
                .DisableAntiforgery()
                .Produces(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
