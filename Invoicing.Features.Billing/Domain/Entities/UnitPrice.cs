namespace Invoicing.Features.Billing.Domain.Entities
{
    public class UnitPrice
    {
        public static UnitPrice CreateUnitPrice(double unitPrice)
        {
            if (unitPrice <= 0)
            {
                throw new ArgumentException("Unit price must have a positive value");
            }

            return new UnitPrice(unitPrice);
        }

        public double Value { get; private set; }

        private UnitPrice(double value)
        {
            Value = value;
        }
    }
}
