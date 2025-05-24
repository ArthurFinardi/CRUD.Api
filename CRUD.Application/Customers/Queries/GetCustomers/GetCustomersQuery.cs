using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CRUD.Application.Common.Interfaces;
using MediatR;

namespace CRUD.Application.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery : IQuery<IEnumerable<GetCustomerDto>>
    {
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<GetCustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<GetCustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync(cancellationToken);

            return customers.Select(customer => new GetCustomerDto
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
                Address = new GetCustomerDto.AddressDto
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
            });
        }
    }
} 