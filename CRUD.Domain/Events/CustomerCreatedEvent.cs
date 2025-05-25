using System;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events
{
    public class CustomerCreatedEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Document { get; }
        public string Email { get; }
        public string Phone { get; }
        public DateTime CreatedAt { get; }

        public CustomerCreatedEvent(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Document = customer.Document;
            Email = customer.Email;
            Phone = customer.Phone;
            CreatedAt = customer.CreatedAt;
        }
    }
} 