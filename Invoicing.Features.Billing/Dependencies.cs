using Invoicing.Features.Billing.Application.Services;
using Invoicing.Features.Billing.Domain.Repositories;
using Invoicing.Features.Billing.Infrastructure.Persistence;
using Invoicing.Features.Billing.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Invoicing.Features.Billing
{
    public static class Dependencies
    {
        public static IServiceCollection InjectBillingDependencies(this IServiceCollection services)
        {
            return services
                .AddScoped<ICustomerQueryService, CustomerQueryService>()
                .AddScoped<IInvoiceRepository, InvoiceRepository>();
        }
    }
}
