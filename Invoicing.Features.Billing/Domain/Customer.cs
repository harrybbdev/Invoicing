namespace Invoicing.Features.Billing.Domain
{
    public class Customer
    {
        public required int Id { get; set; }
        public required Guid UniqueId { get; set; }
        public required string Name { get; set; }
    }
}
