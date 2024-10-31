using Marten;
using MiniVSA.CatalogService.Domain.Entities;

namespace MiniVSA.CatalogService.Infrastructure.Data.Configuration
{
    public class MartenConfiguration : IConfigureMarten
    {
        public void Configure(IServiceProvider services, StoreOptions options)
        {
            options.Schema
                   .For<Brand>()
                   .Identity(x => x.Id);
        }
    }
}
