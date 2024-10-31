using MiniVSA.CatalogService.Application.Interfaces.Entity;
using MiniVSA.CatalogService.Domain.Entities.Common;
using MiniVSA.CatalogService.Domain.Enums;

namespace MiniVSA.CatalogService.Domain.Entities
{
    public class File : BaseEntity<Guid>, ICreatedOn
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public double Size { get; set; }

        public FileType FileType { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
