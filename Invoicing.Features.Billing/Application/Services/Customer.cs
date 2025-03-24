namespace Invoicing.Features.Billing.Application.Services
{
    public class Customer
    {
        public required Guid UniqueId { get; set; }
        public required string Name { get; set; }
    }
}
