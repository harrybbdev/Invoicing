namespace Invoicing.Core.Domain
{
    public class AggregateRoot
    {
        public int Id { get; set; }
        public Guid UniqueId { get; }

        public AggregateRoot(Guid uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}
