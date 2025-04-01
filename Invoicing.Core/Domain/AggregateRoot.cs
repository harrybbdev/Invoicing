
namespace Invoicing.Core.Domain
{
    public class AggregateRoot : Entity
    {
        public List<Event> DomainEvents { get; private set; } = [];

        public AggregateRoot(Guid uniqueId) : base(uniqueId)
        {
        }

        protected void AddDomainEvent(Event domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }
    }
}
