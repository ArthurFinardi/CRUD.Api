using System;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events
{
    public class CustomerUpdatedEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public DateTime UpdatedAt { get; }

        public CustomerUpdatedEvent(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Phone = customer.Phone;
            UpdatedAt = customer.UpdatedAt.Value;
        }
    }
} 