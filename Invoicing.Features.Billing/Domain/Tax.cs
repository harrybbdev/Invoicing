namespace Invoicing.Features.Billing.Domain
{
    public record Tax
    {
        public static Tax CreateTax(double taxPercentage)
        {
            if (taxPercentage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(taxPercentage));
            }

            return new Tax(taxPercentage);
        }

        public double TaxPercentage { get; }

        public Tax(double taxPercentage)
        {
            TaxPercentage = taxPercentage;
        }

        public double ApplyTax(double amount)
        {
            // 20% tax is means a multipler of (20 / 100) + 1 = (0.2) + 1 = 1.2
            var taxMultipler = TaxPercentage / 100 + 1;
            return amount * taxMultipler;
        }
    }
}
