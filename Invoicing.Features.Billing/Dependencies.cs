using Azure.Messaging.ServiceBus;
using Invoicing.Features.Billing.Application.Services;
using Invoicing.Features.Billing.Domain.Repositories;
using Invoicing.Features.Billing.Infrastructure;
using Invoicing.Features.Billing.Infrastructure.DataAccess;
using Invoicing.Features.Billing.Infrastructure.Repositories;
using Invoicing.Features.Billing.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Invoicing.Features.Billing
{
    public static class Dependencies
    {
        public static WebApplicationBuilder InjectBillingDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BillingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetValue<string>("Features:Billing:ConnectionString"), options =>
                {
                    options.EnableRetryOnFailure();
                }));

            builder.Services
                .AddScoped<BillingServiceBusSender>(provider =>
                {
                    var serviceBusClient = provider.GetRequiredService<ServiceBusClient>();
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var serviceBusTopicName = configuration.GetRequiredSection("Features:Billing:ServiceBus:TopicName").Value;
                    return new BillingServiceBusSender(serviceBusClient.CreateSender(serviceBusTopicName));
                })
                .AddScoped<UnitOfWork>()
                .AddScoped<ICustomerQueryService, CustomerQueryService>()
                .AddScoped<IInvoiceRepository, InvoiceRepository>();

            return builder;
        }
    }
}
