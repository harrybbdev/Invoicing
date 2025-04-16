using Invoicing.Features.Billing.Application.UseCases.CreateInvoice;
using Microsoft.AspNetCore.Builder;

namespace Invoicing.Features.Billing
{
    public static class Endpoints
    {
        public static void MapBillingEndpoints(this WebApplication app)
        {
            CreateInvoiceEndpoint.MapEndpoint(app);
        }
    }
}
