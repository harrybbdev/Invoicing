using Azure.Messaging.ServiceBus;
using Invoicing.Features.Billing.Domain.Repositories;
using Invoicing.Features.Billing.Infrastructure;
using Invoicing.Features.Billing.Infrastructure.DataAccess;
using Invoicing.Features.Billing.Infrastructure.Outbox;
using Invoicing.Features.Billing.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Invoicing.Features.Billing
{
    public static class Dependencies
    {
        public static WebApplicationBuilder InjectBillingDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BillingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetValue<string>("Features:Billing:Database:ConnectionString"), options =>
                {
                    options.EnableRetryOnFailure();
                }));

            builder.Services
                .AddHostedService(provider =>
                {
                    var serviceBusClient = provider.GetRequiredService<ServiceBusClient>();
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var logger = provider.GetRequiredService<ILogger<BillingOutboxBackgroundPublisher>>();
                    var serviceBusTopicName = configuration.GetRequiredSection("Features:Billing:ServiceBus:TopicName").Value;
                    return new BillingOutboxBackgroundPublisher(provider, logger, serviceBusClient.CreateSender(serviceBusTopicName));
                })
                .AddScoped<BillingUnitOfWork>()
                .AddScoped<IInvoiceRepository, InvoiceRepository>();

            return builder;
        }
    }
}
