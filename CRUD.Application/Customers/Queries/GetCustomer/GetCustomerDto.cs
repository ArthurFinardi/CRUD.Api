using System;
using CRUD.Domain.Enums;

namespace CRUD.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerType Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public string StateRegistration { get; set; }
        public bool IsStateRegistrationExempt { get; set; }
        public AddressDto Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AddressDto
    {
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
} 