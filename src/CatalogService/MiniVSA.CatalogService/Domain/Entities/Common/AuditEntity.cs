
using MiniVSA.CatalogService.Application.Interfaces.Entity;

namespace MiniVSA.CatalogService.Domain.Entities.Common
{
    public class AuditEntity<T> : IEntity<T>, ICreatedOn, IModifiedOn 
    {
        public T Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
