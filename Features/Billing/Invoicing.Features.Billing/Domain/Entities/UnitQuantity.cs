namespace Invoicing.Features.Billing.Domain.Entities
{
    public class UnitQuantity
    {
        public static UnitQuantity CreateUnitQuantity(int unitQuantity)
        {
            if (unitQuantity <= 0)
            {
                throw new ArgumentException("Unit quantity must have a positive value");
            }

            return new UnitQuantity(unitQuantity);
        }

        public int Value { get; private set; }

        private UnitQuantity(int value)
        {
            Value = value;
        }
    }
}
