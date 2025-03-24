using Invoicing.Features.Billing.Application.Services;

namespace Invoicing.Features.Billing.Infrastructure.Services
{
    public class CustomerQueryService : ICustomerQueryService
    {
        public Task<bool> DoesCustomerExist(Guid customerId)
        {
            return Task.FromResult(true);
        }
    }
}
