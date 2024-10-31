using Mapster;
using Marten;
using MiniVSA.CatalogService.Application.Constants;
using MiniVSA.CatalogService.Application.Interfaces.CQRS.Query;
using MiniVSA.CatalogService.Application.Models.Result;
using MiniVSA.CatalogService.Domain.Entities;
using MiniVSA.CatalogService.Features.Brands.Common.Models;
using System.Net;

namespace MiniVSA.CatalogService.Features.Brands.GetBrandById
{
    public record GetBrandByIdQuery(Guid Id) : IQuery<Result<BrandDto>>;

    public class GetBrandByIdQueryHandler(IDocumentSession documentSession) : IQueryHandler<GetBrandByIdQuery, Result<BrandDto>>
    {
        public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
        {
            var brand = await documentSession.LoadAsync<Brand>(query.Id, token: cancellationToken);

            if (brand is null || brand.Deleted)
                return Result<BrandDto>.Error(ResponseMessageConstants.Brand.Error.BrandNotFound, (int)HttpStatusCode.NotFound);

            var brandDto = brand.Adapt<BrandDto>();
            return Result<BrandDto>.Success(ResponseMessageConstants.Brand.Success.BrandFound, brandDto, (int)HttpStatusCode.OK);
        }
    }
}
