using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Domain.Events;
using CRUD.Domain.Interfaces;
using MediatR;

namespace CRUD.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEventStore _eventStore;

        public DeleteCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IEventStore eventStore)
        {
            _customerRepository = customerRepository;
            _eventStore = eventStore;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                throw new InvalidOperationException("Cliente não encontrado.");

            await _customerRepository.DeleteAsync(customer, cancellationToken);
            await _eventStore.SaveEvent(new CustomerDeletedEvent(customer.Id), "CustomerDeleted");
        }
    }
} 