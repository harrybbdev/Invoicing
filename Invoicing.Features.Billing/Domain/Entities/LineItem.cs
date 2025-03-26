using Invoicing.Core.Domain;

namespace Invoicing.Features.Billing.Domain.Entities
{
    public class LineItem : Entity
    {
        public static LineItem CreateLineItem(
            string description,
            double price,
            int quantity,
            double taxPercentage)
        {
            return new LineItem()
            {
                Description = UnitDescription.CreateUnitDescription(description),
                Price = UnitPrice.CreateUnitPrice(price),
                Quantity = UnitQuantity.CreateUnitQuantity(quantity),
                Tax = Tax.CreateTax(taxPercentage)
            };
        }

        public required UnitDescription Description { get; set; }
        public required UnitPrice Price { get; set; }
        public required UnitQuantity Quantity { get; set; }
        public required Tax Tax { get; set; }

        public double Total => Tax.ApplyTax(Price.Value * Quantity.Value);

        private LineItem() : this(Guid.NewGuid()) { }

        private LineItem(Guid uniqueId) : base(uniqueId) { }
    }
}
