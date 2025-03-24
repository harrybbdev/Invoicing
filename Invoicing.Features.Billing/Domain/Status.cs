namespace Invoicing.Features.Billing.Domain
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
