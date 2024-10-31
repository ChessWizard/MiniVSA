using Marten;
using MediatR;
using MiniVSA.CatalogService.Application.Constants;
using MiniVSA.CatalogService.Application.Interfaces.CQRS.Command;
using MiniVSA.CatalogService.Application.Models.Result;
using MiniVSA.CatalogService.Domain.Entities;
using System.Net;

namespace MiniVSA.CatalogService.Features.Brands.DeleteBrand
{
    public record DeleteBrandCommand(Guid Id) : ICommand;

    public class DeleteBrandCommandHandler(IDocumentSession documentSession) : ICommandHandler<DeleteBrandCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
        {
            var brand = await documentSession.LoadAsync<Brand>(command.Id, token: cancellationToken);

            if (brand is null || brand.Deleted)
                return Result<Unit>.Error(ResponseMessageConstants.Brand.Error.BrandNotFoundForDelete, (int)HttpStatusCode.NotFound);

            brand.SoftDelete();
            documentSession.Update(brand);
            await documentSession.SaveChangesAsync(cancellationToken);

            documentSession.Delete(brand);
            await documentSession.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(ResponseMessageConstants.Brand.Success.BrandDeleted, (int)HttpStatusCode.OK);
        }
    }
}
