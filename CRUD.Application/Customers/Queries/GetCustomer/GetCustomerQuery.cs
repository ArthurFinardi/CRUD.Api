using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Application.Common.Interfaces;
using MediatR;

namespace CRUD.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IQuery<GetCustomerDto>
    {
        public Guid Id { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, GetCustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (customer == null)
            {
                throw new InvalidOperationException($"Cliente com ID {request.Id} não encontrado.");
            }

            return new GetCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Document = customer.Document,
                Email = customer.Email,
                Phone = customer.Phone,
                Type = customer.Type,
                BirthDate = customer.BirthDate,
                StateRegistration = customer.StateRegistration,
                IsStateRegistrationExempt = customer.IsStateRegistrationExempt,
                Address = new AddressDto
                {
                    ZipCode = customer.Address.ZipCode,
                    Street = customer.Address.Street,
                    Number = customer.Address.Number,
                    Neighborhood = customer.Address.Neighborhood,
                    City = customer.Address.City,
                    State = customer.Address.State
                },
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }
    }
} 