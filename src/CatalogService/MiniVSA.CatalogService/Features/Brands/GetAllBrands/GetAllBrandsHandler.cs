using Mapster;
using Marten;
using Marten.Pagination;
using MiniVSA.CatalogService.Application.Constants;
using MiniVSA.CatalogService.Application.Interfaces.CQRS.Query;
using MiniVSA.CatalogService.Application.Models.Common.Request;
using MiniVSA.CatalogService.Application.Models.Result.Paging;
using MiniVSA.CatalogService.Domain.Entities;
using MiniVSA.CatalogService.Features.Brands.Common.Models;
using System.Net;

namespace MiniVSA.CatalogService.Features.Brands.GetAllBrands
{
    public record GetAllBrandsQuery(PaginationRequestModel Pagination) : IQuery<PagingResult<GetAllBrandsQueryResult>>;
    
    public record GetAllBrandsQueryResult(List<BrandDto> Brands);

    public class GetAllBrandsHandler(IDocumentSession documentSession) : IQueryHandler<GetAllBrandsQuery, PagingResult<GetAllBrandsQueryResult>>
    {
        public async Task<PagingResult<GetAllBrandsQueryResult>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var queryBrands = await documentSession.Query<Brand>()
                                        .ToPagedListAsync(request.Pagination.PageNumber, request.Pagination.PageSize, cancellationToken);

            if(queryBrands is null || !queryBrands.Any())
                return PagingResult<GetAllBrandsQueryResult>.Error(ResponseMessageConstants.Brand.Error.AnyBrandNotFound, (int)HttpStatusCode.NotFound);

            var totalBrandCount = await documentSession.Query<Brand>().CountAsync(token: cancellationToken);
            PagingMetaData pagingMetaData = new((int)queryBrands.PageSize, (int)queryBrands.PageNumber, totalBrandCount);
            var brands = queryBrands.Adapt<List<BrandDto>>();
            GetAllBrandsQueryResult result = new(brands);

            return PagingResult<GetAllBrandsQueryResult>.Success(result, pagingMetaData, (int)HttpStatusCode.OK);
        }
    }
}
