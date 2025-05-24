using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Domain.ValueObjects;
using MediatR;

namespace CRUD.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : ICommand<Guid>
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Domain.Enums.CustomerType Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public string StateRegistration { get; set; }
        public bool IsStateRegistrationExempt { get; set; }
        public AddressDto Address { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
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
                request.Address.ZipCode,
                request.Address.Street,
                request.Address.Number,
                request.Address.Neighborhood,
                request.Address.City,
                request.Address.State
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

            return customer.Id;
        }
    }
} 