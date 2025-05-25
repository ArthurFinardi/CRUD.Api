using System;

namespace CRUD.Domain.Events
{
    public class CustomerDeletedEvent
    {
        public Guid Id { get; }
        public DateTime DeletedAt { get; }

        public CustomerDeletedEvent(Guid id)
        {
            Id = id;
            DeletedAt = DateTime.UtcNow;
        }
    }
} 