using System;

namespace CRUD.Domain.ValueObjects
{
    public class Address
    {
        public string ZipCode { get; init; }
        public string Street { get; init; }
        public string Number { get; init; }
        public string Neighborhood { get; init; }
        public string City { get; init; }
        public string State { get; init; }

        public Address(
            string zipCode,
            string street,
            string number,
            string neighborhood,
            string city,
            string state)
        {
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
} 