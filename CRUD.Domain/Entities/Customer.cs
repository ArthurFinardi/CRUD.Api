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
        public string StateRegistration { get; private set; }
        public bool IsStateRegistrationExempt { get; private set; }
        public Address Address { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Customer() { } // For EF Core

        public Customer(
            string name,
            string document,
            string email,
            string phone,
            CustomerType type,
            Address address,
            DateTime? birthDate = null,
            string stateRegistration = null,
            bool isStateRegistrationExempt = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Document = document;
            Email = email;
            Phone = phone;
            Type = type;
            Address = address;
            BirthDate = birthDate;
            StateRegistration = stateRegistration;
            IsStateRegistrationExempt = isStateRegistrationExempt;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string name,
            string phone,
            string email,
            Address address)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 