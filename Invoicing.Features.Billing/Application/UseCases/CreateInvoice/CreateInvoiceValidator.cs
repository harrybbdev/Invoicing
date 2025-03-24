using FluentValidation;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceValidator()
        {
            RuleFor(x => x.CustomerUniqueId).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDeadline).NotEqual(DateTime.MinValue);
            RuleFor(x => x.UniqueId).NotEmpty().NotNull();
            RuleFor(x => x.Currency).IsInEnum();
        }
    }
}
