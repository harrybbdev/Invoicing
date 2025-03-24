namespace Invoicing.Features.Billing.Domain
{
    public class UnitDescription
    {
        public static UnitDescription CreateUnitDescription(string unitDescription)
        {
            if (string.IsNullOrEmpty(unitDescription))
            {
                throw new ArgumentException("Unit description must have a value");
            }

            return new UnitDescription(unitDescription);
        }

        public string Value { get; }

        private UnitDescription(string unitDescription)
        {
            Value = unitDescription;
        }
    }
}
