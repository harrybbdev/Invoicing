using System.Text.Json;
using Invoicing.Core.Application;
using Invoicing.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Core.Infrastructure
{
    public abstract class UnitOfWork<T> : IUnitOfWork<T> where T : DbContext
    {
        private readonly T _dbContext;
        private readonly IMediator _mediator;

        protected abstract HashSet<Type> IntegrationEvents { get; }

        public UnitOfWork(T dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            var events = _dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .SelectMany(e => e.Entity.DomainEvents)
                .OrderBy(ev => ev.DateRaised)
                .ToList();

            foreach (var @event in events)
            {
                await PublishToDomainEventHandlers(@event, cancellationToken);
                AddToOutbox(@event, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        private async Task PublishToDomainEventHandlers(Event @event, CancellationToken cancellationToken)
        {
            await _mediator.Publish(@event, cancellationToken);
        }

        private void AddToOutbox(Event @event, CancellationToken cancellationToken)
        {
            var eventType = @event.GetType();
            if (IntegrationEvents.Contains(eventType))
            {
                _dbContext
                    .Set<OutboxMessage>()
                    .Add(new OutboxMessage(eventType.Name, JsonSerializer.Serialize(@event)));
            }
        }
    }
}
