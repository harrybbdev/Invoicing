using Invoicing.Features.Customers.Contracts.ExternalServices;
using Invoicing.Features.Customers.ExternalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Invoicing.Features.Customers
{
    public static class Dependencies
    {
        public static WebApplicationBuilder InjectCustomerDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICustomerExternalService, CustomerExternalService>();
            return builder;
        }
    }
}
