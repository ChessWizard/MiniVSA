
using MiniVSA.CatalogService.Application.Interfaces.Entity;

namespace MiniVSA.CatalogService.Domain.Entities.Common
{
    public class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; set; }
    }
}
