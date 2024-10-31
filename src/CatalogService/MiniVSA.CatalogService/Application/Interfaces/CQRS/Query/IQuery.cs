using MediatR;

namespace MiniVSA.CatalogService.Application.Interfaces.CQRS.Query
{
    public interface IQuery<out TResponse> : IRequest<TResponse>, IBaseRequest
        where TResponse : notnull
    {
    }

    public interface IQuery : IRequest<Unit>, IBaseRequest
    {
    }
}
