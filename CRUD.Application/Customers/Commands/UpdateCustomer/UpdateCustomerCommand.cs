using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using CRUD.Application.Interfaces;
using CRUD.Domain.ValueObjects;
using CRUD.Domain.Interfaces;
using MediatR;

namespace CRUD.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string ZipCode { get; set; }
        public required string Street { get; set; }
        public required string Number { get; set; }
        public required string Neighborhood { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
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

            var updatedCustomer = customer.Update(
                request.Name,
                request.Phone,
                request.Email,
                address
            );

            await _customerRepository.UpdateAsync(updatedCustomer, cancellationToken);
            await _eventStore.SaveEvent(new CustomerUpdatedEvent(updatedCustomer), "CustomerUpdated");
        }
    }
} 