namespace Invoicing.Core.Domain
{
    public class AggregateRoot
    {
        public List<Event> DomainEvents { get; private set; } = [];

        protected void AddDomainEvent(Event domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }
    }
}
