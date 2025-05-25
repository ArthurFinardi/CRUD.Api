using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD.Domain.Interfaces
{
    public interface IEventStore
    {
        Task SaveEvent<T>(T @event, string eventType) where T : class;
        Task<IEnumerable<object>> GetEvents(Guid aggregateId);
    }
} 