using Invoicing.Features.Customers.Contracts.ExternalServices;

namespace Invoicing.Features.Customers.ExternalServices
{
    public class CustomerExternalService : ICustomerExternalService
    {
        public Task<bool> DoesCustomerExist(Guid customerId)
        {
            return Task.FromResult(true);
        }
    }
}
