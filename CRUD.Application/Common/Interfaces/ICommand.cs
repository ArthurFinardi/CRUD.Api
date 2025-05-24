using MediatR;

namespace CRUD.Application.Common.Interfaces
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
} 