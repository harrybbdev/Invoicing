using Invoicing.Core.Domain;
using MediatR;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceCommand : IRequest<CreateInvoiceResponse>
    {
        public class LineItem
        {
            public Guid? UniqueId { get; set; }
            public required string Description { get; set; }
            public required double UnitPrice { get; set; }
            public required int UnitQuantity { get; set; }
            public required double TaxPercentage { get; set; }
        }

        public Guid? UniqueId { get; set; }
        public required DateTime PaymentDeadline { get; set; }
        public required List<LineItem> LineItems { get; set; }
        public required Currency Currency { get; set; }
        public required Guid CustomerUniqueId { get; set; }
    }
}
