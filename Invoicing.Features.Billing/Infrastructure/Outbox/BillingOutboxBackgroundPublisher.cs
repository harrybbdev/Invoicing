using Azure.Messaging.ServiceBus;
using Invoicing.Core.Domain;
using Invoicing.Core.Infrastructure.Outbox;
using Invoicing.Features.Billing.Infrastructure.DataAccess;
using Microsoft.Extensions.Logging;

namespace Invoicing.Features.Billing.Infrastructure.Outbox
{
    public class BillingOutboxBackgroundPublisher : OutboxBackgroundPublisher<BillingDbContext>
    {
        private readonly ServiceBusSender _sender;

        public BillingOutboxBackgroundPublisher(
            IServiceProvider serviceProvider,
            ILogger<OutboxBackgroundPublisher<BillingDbContext>> logger,
            ServiceBusSender sender) : base(serviceProvider, logger)
        {
            _sender = sender;
        }

        protected override async Task ProcessOutboxMessage(OutboxMessage outboxMessage, CancellationToken cancellationToken)
        {
            var message = new ServiceBusMessage(outboxMessage.Payload)
            {
                Subject = outboxMessage.Payload,
                MessageId = Guid.NewGuid().ToString()
            };

            await _sender.SendMessageAsync(message, cancellationToken);
        }
    }
}
