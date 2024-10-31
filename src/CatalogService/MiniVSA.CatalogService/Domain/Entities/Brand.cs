using Marten.Metadata;
using MiniVSA.CatalogService.Domain.Entities.Common;

namespace MiniVSA.CatalogService.Domain.Entities
{
    public class Brand : AuditEntity<Guid>, ISoftDeleted
    {
        public string Name { get; set; }

        public ICollection<File> Files { get; set; } = [];

        public bool Deleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }

        public void SoftDelete()
        {
            Deleted = true;
            DeletedAt = DateTimeOffset.Now;
        }
    }
}
