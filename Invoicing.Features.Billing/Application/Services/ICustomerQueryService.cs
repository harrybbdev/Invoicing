namespace Invoicing.Features.Billing.Application.Services
{
    public interface ICustomerQueryService
    {
        Task<bool> DoesCustomerExist(Guid customerId);
    }
}
