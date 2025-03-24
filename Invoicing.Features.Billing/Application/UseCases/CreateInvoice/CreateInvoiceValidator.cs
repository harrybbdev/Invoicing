using FluentValidation;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceValidator()
        {
            RuleForEach(x => x.LineItems).ChildRules(items =>
            {
                items.RuleFor(li => li.UnitPrice).GreaterThan(0);
                items.RuleFor(li => li.UnitQuantity).GreaterThan(0);
                items.RuleFor(li => li.Description).MinimumLength(1);
                items.RuleFor(li => li.TaxPercentage).LessThan(1).GreaterThanOrEqualTo(0);
            });
            RuleFor(x => x.CustomerUniqueId).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDeadline).NotEqual(DateTime.MinValue);
            RuleFor(x => x.UniqueId).NotEmpty().NotNull();
            RuleFor(x => x.Currency).IsInEnum();
        }
    }
}
