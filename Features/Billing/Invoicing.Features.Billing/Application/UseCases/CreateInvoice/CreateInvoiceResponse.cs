using Invoicing.Core.Domain;
using Invoicing.Features.Billing.Domain.Entities;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceResponse
    {
        public class LineItem
        {
            public required Guid UniqueId { get; set; }
            public required string Description { get; set; }
            public required double UnitPrice { get; set; }
            public required int UnitQuantity { get; set; }
            public required double TaxPercentage { get; set; }
        }

        public required Guid UniqueId { get; set; }
        public required DateTime PaymentDeadline { get; set; }
        public required List<LineItem> LineItems { get; set; }
        public required Status Status { get; set; }
        public required Currency Currency { get; set; }
        public required Guid CustomerUniqueId { get; set; }
        public required double Total { get; set; }
        public required double AmountPaid { get; set; }
        public required double AmountOutstanding { get; set; }
    }
}
