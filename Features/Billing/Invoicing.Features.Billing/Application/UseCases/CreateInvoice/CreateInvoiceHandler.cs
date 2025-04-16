using Invoicing.Core.Exceptions;
using Invoicing.Features.Billing.Domain.Entities;
using Invoicing.Features.Billing.Domain.Repositories;
using Invoicing.Features.Billing.Infrastructure;
using Invoicing.Features.Customers.Contracts.ExternalServices;
using MediatR;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponse>
    {
        private readonly ICustomerExternalService _customerService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly BillingUnitOfWork _unitOfWork;

        public CreateInvoiceHandler(
            ICustomerExternalService customerQueryService,
            IInvoiceRepository invoiceRepository,
            BillingUnitOfWork unitOfWork)
        {
            _customerService = customerQueryService;
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateInvoiceResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var doesCustomerExist = await _customerService.DoesCustomerExist(request.CustomerUniqueId);
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

            await _unitOfWork.SaveChanges(cancellationToken);

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
