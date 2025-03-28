using MediatR;

namespace Invoicing.Core.Domain
{
    public class Event : INotification
    {
        public DateTime DateRaised { get; } = DateTime.UtcNow;
    }
}
