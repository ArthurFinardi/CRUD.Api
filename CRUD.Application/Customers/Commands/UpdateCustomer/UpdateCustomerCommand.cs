using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.ValueObjects;
using MediatR;

namespace CRUD.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public AddressDto Address { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (customer == null)
            {
                throw new InvalidOperationException($"Cliente com ID {request.Id} não encontrado.");
            }

            // Verificar se o novo e-mail já está em uso por outro cliente
            if (customer.Email != request.Email)
            {
                var existingCustomerByEmail = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);
                if (existingCustomerByEmail != null && existingCustomerByEmail.Id != request.Id)
                {
                    throw new InvalidOperationException("Já existe um cliente cadastrado com este e-mail.");
                }
            }

            var address = new Address(
                request.Address.ZipCode,
                request.Address.Street,
                request.Address.Number,
                request.Address.Neighborhood,
                request.Address.City,
                request.Address.State
            );

            customer.Update(
                request.Name,
                request.Phone,
                request.Email,
                address
            );

            await _customerRepository.UpdateAsync(customer, cancellationToken);

            return true;
        }
    }
} 