namespace MiniVSA.CatalogService.Application.Interfaces.Entity
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
