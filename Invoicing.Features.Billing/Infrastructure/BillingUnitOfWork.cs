using Invoicing.Core.Infrastructure;
using Invoicing.Features.Billing.Domain.Events;
using Invoicing.Features.Billing.Infrastructure.DataAccess;
using MediatR;

namespace Invoicing.Features.Billing.Infrastructure
{
    public class BillingUnitOfWork : UnitOfWork<BillingDbContext>
    {
        protected override HashSet<Type> IntegrationEvents => [
            typeof(InvoiceCreatedEvent)
        ];

        public BillingUnitOfWork(BillingDbContext dbContext, IMediator mediator) : base(dbContext, mediator) { }
    }
}
