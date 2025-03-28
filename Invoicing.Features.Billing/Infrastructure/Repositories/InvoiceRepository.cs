using Invoicing.Features.Billing.Domain.Entities;
using Invoicing.Features.Billing.Domain.Repositories;
using Invoicing.Features.Billing.Infrastructure.DataAccess;

namespace Invoicing.Features.Billing.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly BillingDbContext _billingDbContext;

        public InvoiceRepository(BillingDbContext billingDbContext)
        {
            _billingDbContext = billingDbContext;
        }

        public async Task AddInvoice(Invoice invoice)
        {
            _billingDbContext.Invoices.Add(invoice);
            await Task.CompletedTask;
        }
    }
}
