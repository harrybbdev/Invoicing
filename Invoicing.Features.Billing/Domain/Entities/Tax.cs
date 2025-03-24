namespace Invoicing.Features.Billing.Domain.Entities
{
    public record Tax
    {
        public static Tax CreateTax(double taxPercentage)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(taxPercentage);
            return new Tax(taxPercentage);
        }

        public double TaxPercentage { get; }

        public Tax(double taxPercentage)
        {
            TaxPercentage = taxPercentage;
        }

        public double ApplyTax(double amount)
        {
            // 20% tax means a multipler of 0.2 + 1 = 1.2
            var taxMultipler = TaxPercentage + 1;
            return amount * taxMultipler;
        }
    }
}
