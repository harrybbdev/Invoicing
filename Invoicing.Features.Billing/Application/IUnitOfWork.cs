namespace Invoicing.Features.Billing.Application
{
    public interface IUnitOfWork
    {
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
