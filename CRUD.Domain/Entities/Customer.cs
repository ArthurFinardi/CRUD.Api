using System;
using CRUD.Domain.ValueObjects;
using CRUD.Domain.Enums;

namespace CRUD.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public CustomerType Type { get; private set; }
        public string? StateRegistration { get; private set; }
        public bool IsStateRegistrationExempt { get; private set; }
        public Address Address { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Customer() { } // For EF Core

        public static Customer Create(
            string name,
            string document,
            string email,
            string phone,
            CustomerType type,
            Address address,
            DateTime? birthDate = null,
            string? stateRegistration = null,
            bool isStateRegistrationExempt = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(document))
                throw new ArgumentException("Document cannot be empty", nameof(document));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be empty", nameof(phone));
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            return new Customer
            {
                Id = Guid.NewGuid(),
                Name = name,
                Document = document,
                Email = email,
                Phone = phone,
                Type = type,
                Address = address,
                BirthDate = birthDate,
                StateRegistration = stateRegistration,
                IsStateRegistrationExempt = isStateRegistrationExempt,
                CreatedAt = DateTime.UtcNow
            };
        }

        public Customer Update(
            string name,
            string phone,
            string email,
            Address address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be empty", nameof(phone));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            var updatedCustomer = Create(
                name,
                Document,
                email,
                phone,
                Type,
                address,
                BirthDate,
                StateRegistration,
                IsStateRegistrationExempt);

            updatedCustomer.Id = Id;
            updatedCustomer.CreatedAt = CreatedAt;
            updatedCustomer.UpdatedAt = DateTime.UtcNow;

            return updatedCustomer;
        }
    }
} 