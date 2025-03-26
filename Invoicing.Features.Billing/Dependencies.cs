using Invoicing.Features.Billing.Application.Services;
using Invoicing.Features.Billing.Domain.Repositories;
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
                .AddScoped<ICustomerQueryService, CustomerQueryService>()
                .AddScoped<IInvoiceRepository, InvoiceRepository>();

            return builder;
        }
    }
}
