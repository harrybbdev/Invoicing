namespace Invoicing.Features.Billing.Domain.Entities
{
    public class LineItem
    {
        public Guid UniqueId { get; }
        public UnitDescription Description { get; set; }
        public UnitPrice Price { get; set; }
        public UnitQuantity Quantity { get; set; }
        public Tax Tax { get; set; }

        public double Total => Tax.ApplyTax(Price.Value * Quantity.Value);

        public LineItem(
            UnitDescription description,
            UnitPrice price,
            UnitQuantity quantity,
            Tax tax) : this(Guid.NewGuid(), description, price, quantity, tax) { }

        private LineItem(
            Guid uniqueId,
            UnitDescription description,
            UnitPrice price,
            UnitQuantity quantity,
            Tax tax)
        {
            UniqueId = uniqueId;
            Tax = default!;
            Quantity = default!;
            Price = default!;
            Description = default!;
        }
    }
}
