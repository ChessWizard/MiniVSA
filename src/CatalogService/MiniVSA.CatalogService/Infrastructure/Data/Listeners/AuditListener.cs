using Marten;
using Marten.Metadata;
using Marten.Services;
using MiniVSA.CatalogService.Application.Interfaces.Entity;

namespace MiniVSA.CatalogService.Infrastructure.Data.Listeners
{
    public class AuditListener : IDocumentSessionListener
    {
        public Task BeforeSaveChangesAsync(IDocumentSession session, CancellationToken token)
        {
            foreach (var entity in session.PendingChanges.Updates().OfType<IModifiedOn>())
            {
                entity.ModifiedDate = DateTime.UtcNow;
            }

            foreach (var entity in session.PendingChanges.Inserts().OfType<ICreatedOn>())
            {
                entity.CreatedDate = DateTime.UtcNow;
            }
            return Task.CompletedTask;
        }

        public void BeforeSaveChanges(IDocumentSession session)
        {
        }

        public void AfterCommit(IDocumentSession session, IChangeSet commit)
        {
        }

        public Task AfterCommitAsync(IDocumentSession session, IChangeSet commit, CancellationToken token)
           => Task.CompletedTask;
        

        public void DocumentAddedForStorage(object id, object document)
        {
        }

        public void DocumentLoaded(object id, object document)
        {
        }
    }
}
