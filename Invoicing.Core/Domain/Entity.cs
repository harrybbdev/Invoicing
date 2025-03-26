namespace Invoicing.Core.Domain
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid UniqueId { get; }

        public Entity(Guid uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}
