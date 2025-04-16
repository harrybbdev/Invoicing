namespace Invoicing.Features.Billing.Domain.Entities
{
    public enum Status
    {
        Draft,
        Sent,
        Paid,
        Overdue,
        Cancelled
    }
}
