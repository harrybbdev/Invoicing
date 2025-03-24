using Invoicing.Features.Billing.Domain.Entities;

namespace Invoicing.Features.Billing.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddInvoice(Invoice invoice);
    }
}
