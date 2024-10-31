using Mapster;
using Marten.Pagination;
using MiniVSA.CatalogService.Domain.Entities;
using MiniVSA.CatalogService.Domain.Enums;
using MiniVSA.CatalogService.Features.Brands.Common.Models;
using MiniVSA.CatalogService.Features.Brands.CreateBrand;

namespace MiniVSA.CatalogService.Application.Mappings
{
    public class BrandMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateBrandRequest, CreateBrandCommand>()
                .ConstructUsing(source => new CreateBrandCommand(source.Name, source.FileUploadModel));

            config.NewConfig<IPagedList<Brand>, List<BrandDto>>()
                  .MapWith(source => source.Select(item => new BrandDto(item.Id, 
                                                                        item.Name, 
                                                                        item.Files
                                                                            .Where(file => file.FileType == FileType.Image)
                                                                            .Select(file => file.Path)
                                                                            .FirstOrDefault()))
                                                                                .ToList());
        }
    }
}
