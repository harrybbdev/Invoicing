namespace Invoicing.Features.Customers.Contracts.ExternalServices
{
    public interface ICustomerExternalService
    {
        Task<bool> DoesCustomerExist(Guid customerId);
    }
}
