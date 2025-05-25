using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using CRUD.Domain.Interfaces;
using CRUD.Domain.ValueObjects;
using MediatR;

namespace CRUD.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEventStore _eventStore;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IEventStore eventStore)
        {
            _customerRepository = customerRepository;
            _eventStore = eventStore;
        }

        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                throw new InvalidOperationException("Cliente não encontrado.");

            // Verificar se o novo email já está em uso por outro cliente
            if (customer.Email != request.Email)
            {
                var existingCustomerByEmail = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);
                if (existingCustomerByEmail != null)
                    throw new InvalidOperationException("Já existe um cliente cadastrado com este e-mail.");
            }

            var address = new Address(
                request.ZipCode,
                request.Street,
                request.Number,
                request.Neighborhood,
                request.City,
                request.State
            );

            customer.Update(
                request.Name,
                request.Phone,
                request.Email,
                address
            );

            await _customerRepository.UpdateAsync(customer, cancellationToken);
            await _eventStore.SaveEvent(new CustomerUpdatedEvent(customer), "CustomerUpdated");
        }
    }
} 