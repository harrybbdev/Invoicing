using Invoicing.Core.Domain;

namespace Invoicing.Core.Infrastructure.Outbox
{
    public class OutboxMessage : Entity
    {
        public DateTime DateOccurred { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime? DatePublished { get; set; }
        public bool IsPublished => DatePublished != null;

        public OutboxMessage(
            DateTime dateOccurred,
            string type,
            string payload,
            DateTime? datePublished)
            : this(Guid.NewGuid(), dateOccurred, type, payload, datePublished) { }

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
