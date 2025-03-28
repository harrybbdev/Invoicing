using System.Text.Json;
using System.Transactions;
using Azure.Messaging.ServiceBus;
using Invoicing.Core.Domain;
using Invoicing.Features.Billing.Domain.Events;
using Invoicing.Features.Billing.Infrastructure.DataAccess;
using MediatR;

namespace Invoicing.Features.Billing.Application
{
    public class UnitOfWork
    {
        private readonly BillingDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly HashSet<Type> _integrationEvents =
        [
            typeof(InvoiceCreatedEvent)
        ];

        public UnitOfWork(BillingDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _dbContext.SaveChangesAsync(cancellationToken);

                var Events = _dbContext.ChangeTracker
                    .Entries<AggregateRoot>()
                    .SelectMany(e => e.Entity.DomainEvents)
                    .OrderBy(ev => ev.DateRaised)
                    .ToList();

                foreach (var @event in Events)
                {
                    await _mediator.Publish(@event, cancellationToken);

                    if (_integrationEvents.Contains(@event.GetType()))
                    {
                        var message = new ServiceBusMessage(JsonSerializer.Serialize(@event))
                        {
                            Subject = @event.GetType().Name,
                            MessageId = Guid.NewGuid().ToString()
                        };

                        await _mediator.SendMessageAsync(message, cancellationToken);
                    }
                }

                scope.Complete();

            };

            return result;

            await _dbContext.SaveChangesAsync();
        }
    }
}
