using System;
using CRUD.Domain.Enums;

namespace CRUD.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerDto
    {
        public required string Name { get; set; }
        public required string Document { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required CustomerType Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? StateRegistration { get; set; }
        public required bool IsStateRegistrationExempt { get; set; }
        public required AddressDto Address { get; set; }
    }

    public class AddressDto
    {
        public required string ZipCode { get; set; }
        public required string Street { get; set; }
        public required string Number { get; set; }
        public required string Neighborhood { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
    }
} 