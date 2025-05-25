using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Domain.Entities;
using CRUD.Domain.Enums;
using CRUD.Domain.Events;
using CRUD.Domain.Interfaces;
using CRUD.Domain.ValueObjects;
using MediatR;

namespace CRUD.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerType Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public string StateRegistration { get; set; }
        public bool IsStateRegistrationExempt { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEventStore _eventStore;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IEventStore eventStore)
        {
            _customerRepository = customerRepository;
            _eventStore = eventStore;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Verificar se já existe um cliente com o mesmo documento
            var existingCustomerByDocument = await _customerRepository.GetByDocumentAsync(request.Document, cancellationToken);
            if (existingCustomerByDocument != null)
            {
                throw new InvalidOperationException("Já existe um cliente cadastrado com este CPF/CNPJ.");
            }

            // Verificar se já existe um cliente com o mesmo e-mail
            var existingCustomerByEmail = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingCustomerByEmail != null)
            {
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

            var customer = new Customer(
                request.Name,
                request.Document,
                request.Email,
                request.Phone,
                request.Type,
                address,
                request.BirthDate,
                request.StateRegistration,
                request.IsStateRegistrationExempt
            );

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _eventStore.SaveEvent(new CustomerCreatedEvent(customer), "CustomerCreated");

            return customer.Id;
        }
    }
} 