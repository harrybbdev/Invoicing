using Invoicing.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Invoicing.Core.Infrastructure.Outbox
{
    public abstract class OutboxBackgroundPublisher<T> : BackgroundService, IDbContextScoped<T> where T : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxBackgroundPublisher<T>> _logger;

        public OutboxBackgroundPublisher(IServiceProvider serviceProvider, ILogger<OutboxBackgroundPublisher<T>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<T>();

                var messages = await db.Set<OutboxMessage>()
                    .Where(x => x.DatePublished == null)
                    .OrderBy(x => x.DateOccurred)
                    .Take(10)
                    .ToListAsync(cancellationToken);

                foreach (var message in messages)
                {
                    try
                    {
                        await ProcessOutboxMessage(message, cancellationToken);

                        message.DatePublished = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error publishing message: {MessageId}", message.Id);
                    }
                }

                await db.SaveChangesAsync(cancellationToken);
                await Task.Delay(1000, cancellationToken); // configurable interval
            }
        }

        protected abstract Task ProcessOutboxMessage(OutboxMessage message, CancellationToken cancellationToken);
    }
}
