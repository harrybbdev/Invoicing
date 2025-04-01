using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Invoicing.Core.Domain;

namespace Invoicing.Features.Billing.Infrastructure.DataAccess
{
    public class BillingServiceBusSender
    {
        private readonly ServiceBusSender _sender;

        public BillingServiceBusSender(ServiceBusSender sender)
        {
            _sender = sender;
        }

        public async Task PublishAsync(Event @event, CancellationToken cancellationToken)
        {
            var message = new ServiceBusMessage(JsonSerializer.Serialize(@event))
            {
                Subject = @event.GetType().Name,
                MessageId = Guid.NewGuid().ToString()
            };

            await _sender.SendMessageAsync(message, cancellationToken);
        }
    }
}
