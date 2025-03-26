using Invoicing.Core.Exceptions;
using Invoicing.Features.Billing.Application.Services;
using Invoicing.Features.Billing.Domain.Entities;
using Invoicing.Features.Billing.Domain.Repositories;
using MediatR;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponse>
    {
        private readonly ICustomerQueryService _customerQueryService;
        private readonly IInvoiceRepository _invoiceRepository;

        public CreateInvoiceHandler(
            ICustomerQueryService customerQueryService,
            IInvoiceRepository invoiceRepository)
        {
            _customerQueryService = customerQueryService;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CreateInvoiceResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var doesCustomerExist = await _customerQueryService.DoesCustomerExist(request.CustomerUniqueId);
            if (!doesCustomerExist)
            {
                throw new NotFoundException($"Customer with ID {request.CustomerUniqueId} could not be found.");
            }

            var lineItems = request.LineItems.Select(li =>
            {
                return LineItem.CreateLineItem(
                    li.Description,
                    li.UnitPrice,
                    li.UnitQuantity,
                    li.TaxPercentage);
            });

            var invoice = Invoice.CreateInvoice(
                request.UniqueId ?? Guid.NewGuid(),
                request.PaymentDeadline,
                lineItems.ToList(),
                request.Currency,
                request.CustomerUniqueId);

            await _invoiceRepository.AddInvoice(invoice);

            return new CreateInvoiceResponse()
            {
                UniqueId = invoice.UniqueId,
                AmountPaid = invoice.AmountPaid,
                Currency = invoice.Currency,
                CustomerUniqueId = invoice.CustomerUniqueId,
                LineItems = invoice.LineItems.Select(li =>
                {
                    return new CreateInvoiceResponse.LineItem()
                    {
                        UniqueId = li.UniqueId,
                        Description = li.Description.Value,
                        TaxPercentage = li.Tax.TaxPercentage,
                        UnitPrice = li.Price.Value,
                        UnitQuantity = li.Quantity.Value,
                    };
                }).ToList(),
                PaymentDeadline = invoice.PaymentDeadline,
                Status = invoice.Status,
                AmountOutstanding = invoice.AmountOutstanding,
                Total = invoice.Total
            };
        }
    }
}
