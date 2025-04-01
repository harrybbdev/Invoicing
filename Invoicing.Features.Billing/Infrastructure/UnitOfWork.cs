using System.Transactions;
using Invoicing.Core.Domain;
using Invoicing.Features.Billing.Application;
using Invoicing.Features.Billing.Domain.Events;
using Invoicing.Features.Billing.Infrastructure.DataAccess;
using MediatR;

namespace Invoicing.Features.Billing.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BillingDbContext _dbContext;
        private readonly BillingServiceBusSender _serviceBusSender;
        private readonly IMediator _mediator;
        private readonly HashSet<Type> _integrationEvents =
        [
            typeof(InvoiceCreatedEvent)
        ];

        public UnitOfWork(
            BillingDbContext dbContext,
            BillingServiceBusSender serviceBusSender,
            IMediator mediator)
        {
            _dbContext = dbContext;
            _serviceBusSender = serviceBusSender;
            _mediator = mediator;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _dbContext.SaveChangesAsync(cancellationToken);

                var events = _dbContext.ChangeTracker
                    .Entries<AggregateRoot>()
                    .SelectMany(e => e.Entity.DomainEvents)
                    .OrderBy(ev => ev.DateRaised)
                    .ToList();

                foreach (var @event in events)
                {
                    await PublishToDomainEventHandlers(@event, cancellationToken);
                    await PublishToIntegrationEventHandlers(@event, cancellationToken);
                }

                scope.Complete();
            };
        }
        private async Task PublishToDomainEventHandlers(Event @event, CancellationToken cancellationToken)
        {
            await _mediator.Publish(@event, cancellationToken);
        }

        private async Task PublishToIntegrationEventHandlers(Event @event, CancellationToken cancellationToken)
        {
            if (_integrationEvents.Contains(@event.GetType()))
            {
                await _serviceBusSender.PublishAsync(@event, cancellationToken);
            }
        }
    }
}
