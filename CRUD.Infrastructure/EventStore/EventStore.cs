using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using CRUD.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using CRUD.Infrastructure.Data;

namespace CRUD.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly ApplicationDbContext _context;

        public EventStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveEvent<T>(T @event, string eventType) where T : class
        {
            var eventData = new EventData
            {
                Id = Guid.NewGuid(),
                AggregateId = GetAggregateId(@event),
                EventType = eventType,
                Data = JsonSerializer.Serialize(@event),
                Timestamp = DateTime.UtcNow
            };

            await _context.EventStore.AddAsync(eventData);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetEvents(Guid aggregateId)
        {
            var events = await _context.EventStore
                .Where(e => e.AggregateId == aggregateId)
                .OrderBy(e => e.Timestamp)
                .ToListAsync();

            return events.Select(e => JsonSerializer.Deserialize<object>(e.Data));
        }

        private Guid GetAggregateId<T>(T @event) where T : class
        {
            var property = @event.GetType().GetProperty("Id");
            if (property == null)
                throw new InvalidOperationException("Event must have an Id property");

            return (Guid)property.GetValue(@event);
        }
    }

    public class EventData
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 