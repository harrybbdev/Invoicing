namespace Invoicing.Features.Customers.Contracts.ExternalServices
{
    public class CustomerDTO
    {
        public required Guid UniqueId { get; set; }
        public required string Name { get; set; }
    }
}
