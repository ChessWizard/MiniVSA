using MediatR;
using MiniVSA.CatalogService.Application.Models.Result;

namespace MiniVSA.CatalogService.Application.Interfaces.CQRS.Command
{
    public interface ICommand<out TResponse> : IRequest<TResponse>, IBaseRequest
    {
    }

    public interface ICommand : ICommand<Result<Unit>>, IBaseRequest
    {
    }
}
