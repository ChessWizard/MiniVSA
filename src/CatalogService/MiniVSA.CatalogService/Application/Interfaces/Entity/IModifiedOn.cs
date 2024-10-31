namespace MiniVSA.CatalogService.Application.Interfaces.Entity
{
    public interface IModifiedOn
    {
        DateTimeOffset? ModifiedDate { get; set; }
    }
}
