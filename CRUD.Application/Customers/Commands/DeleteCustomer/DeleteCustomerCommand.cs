using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Application.Common.Interfaces;
using MediatR;

namespace CRUD.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (customer == null)
            {
                throw new InvalidOperationException($"Cliente com ID {request.Id} não encontrado.");
            }

            await _customerRepository.DeleteAsync(request.Id, cancellationToken);

            return true;
        }
    }
} 