namespace Invoicing.Features.Billing.Domain.Entities
{
    public class LineItem
    {
        public Guid UniqueId { get; }
        public UnitDescription Description { get; set; }
        public UnitPrice UnitPrice { get; set; }
        public UnitQuantity UnitQuantity { get; set; }
        public Tax Tax { get; set; }

        public double Total => Tax.ApplyTax(UnitPrice.Value * UnitQuantity.Value);

        public LineItem(
            UnitDescription description,
            UnitPrice unitPrice,
            UnitQuantity unitQuantity,
            Tax tax)
            : this(Guid.NewGuid(), description, unitPrice, unitQuantity, tax) { }

        private LineItem(
            Guid uniqueId,
            UnitDescription description,
            UnitPrice unitPrice,
            UnitQuantity unitQuantity,
            Tax tax)
        {
            UniqueId = uniqueId;
            Description = description;
            UnitPrice = unitPrice;
            UnitQuantity = unitQuantity;
            Tax = tax;
        }
    }
}
