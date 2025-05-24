using MediatR;

namespace CRUD.Application.Common.Interfaces
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
} 