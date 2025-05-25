using System;
using System.Threading;
using System.Threading.Tasks;
using CRUD.Domain.Interfaces;
using MediatR;

namespace CRUD.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<GetCustomerViewModel>
    {
        public Guid Id { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, GetCustomerViewModel>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomerViewModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                return null;

            return new GetCustomerViewModel
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
                Address = new AddressViewModel
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

    public class GetCustomerViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public string StateRegistration { get; set; }
        public bool IsStateRegistrationExempt { get; set; }
        public AddressViewModel Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AddressViewModel
    {
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
} 