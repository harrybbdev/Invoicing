namespace Invoicing.Core.Domain
{
    public class OutboxMessage : Entity
    {
        public DateTime DateOccurred { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime? DatePublished { get; set; }
        public bool IsPublished => DatePublished != null;

        public OutboxMessage(
            string type,
            string payload)
            : this(Guid.NewGuid(), DateTime.UtcNow, type, payload, null) { }

        private OutboxMessage(
            Guid uniqueId,
            DateTime dateOccurred,
            string type,
            string payload,
            DateTime? datePublished) : base(uniqueId)
        {
            DateOccurred = dateOccurred;
            Type = type;
            Payload = payload;
            DatePublished = datePublished;
        }
    }
}
