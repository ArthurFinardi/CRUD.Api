using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Application.Interfaces;
using CRUD.Application.Common.ViewModels;
using MediatR;

namespace CRUD.Application.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery : IRequest<GetCustomersViewModel>
    {
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, GetCustomersViewModel>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomersViewModel> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync(cancellationToken);
            
            return new GetCustomersViewModel
            {
                Customers = customers.Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Document = c.Document,
                    Email = c.Email,
                    Phone = c.Phone,
                    Type = c.Type.ToString(),
                    BirthDate = c.BirthDate,
                    StateRegistration = c.StateRegistration,
                    IsStateRegistrationExempt = c.IsStateRegistrationExempt,
                    Address = new AddressViewModel
                    {
                        ZipCode = c.Address.ZipCode,
                        Street = c.Address.Street,
                        Number = c.Address.Number,
                        Neighborhood = c.Address.Neighborhood,
                        City = c.Address.City,
                        State = c.Address.State
                    },
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList()
            };
        }
    }

    public class GetCustomersViewModel
    {
        public required List<CustomerViewModel> Customers { get; set; }
    }

    public class CustomerViewModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Document { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? StateRegistration { get; set; }
        public required bool IsStateRegistrationExempt { get; set; }
        public required AddressViewModel Address { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 