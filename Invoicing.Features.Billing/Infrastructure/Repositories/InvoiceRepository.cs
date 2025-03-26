using Invoicing.Features.Billing.Domain.Entities;
using Invoicing.Features.Billing.Domain.Repositories;

namespace Invoicing.Features.Billing.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public async Task AddInvoice(Invoice invoice)
        {
            await Task.CompletedTask;
        }
    }
}
