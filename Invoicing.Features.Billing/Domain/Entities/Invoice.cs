using Invoicing.Core.Domain;

namespace Invoicing.Features.Billing.Domain.Entities
{
    public class Invoice : Entity
    {
        public static Invoice CreateInvoice(
            Guid uniqueId,
            DateTime paymentDeadline,
            List<LineItem> lineItems,
            Currency currency,
            Guid customerUniqueId)
        {
            return new Invoice(
                uniqueId,
                DateTime.MinValue,
                paymentDeadline,
                null,
                Status.Draft,
                currency,
                customerUniqueId)
            {
                LineItems = lineItems
            };
        }

        public static Invoice CreateInvoice(Guid customerUniqueId)
        {
            return new Invoice(
                Guid.NewGuid(),
                DateTime.MinValue,
                DateTime.MinValue,
                null,
                Status.Draft,
                Currency.GBP,
                customerUniqueId);
        }

        public DateTime IssueDate { get; private set; }
        public DateTime PaymentDeadline { get; private set; }
        public List<LineItem> LineItems { get; private set; }
        public Status Status { get; private set; }
        public Currency Currency { get; private set; }

        public string? CancellationReason { get; private set; }

        public Guid CustomerUniqueId { get; private set; }

        public double Total => LineItems.Sum(x => x.Total);
        public double AmountPaid { get; set; }
        public double AmountOutstanding => Total - AmountPaid;

        private Invoice(
            Guid uniqueId,
            DateTime issueDate,
            DateTime paymentDeadline,
            string? cancellationReason,
            Status status,
            Currency currency,
            Guid customerUniqueId) : base(uniqueId)
        {
            IssueDate = issueDate;
            PaymentDeadline = paymentDeadline;
            Status = status;
            LineItems = [];
            Currency = currency;
            Status = status;
            Currency = currency;
            CustomerUniqueId = customerUniqueId;
        }

        public void Update(
            DateTime? paymentDeadline,
            Currency? currency,
            Guid? customerUniqueId)
        {
            if (Status != Status.Draft)
            {
                throw new Exception("An invoice can only be edited in draft mode.");
            }

            if (paymentDeadline != null)
            {
                PaymentDeadline = paymentDeadline.Value;
            }

            if (currency != null)
            {
                Currency = currency.Value;
            }

            if (customerUniqueId != null)
            {
                CustomerUniqueId = customerUniqueId.Value;
            }
        }

        public void AddLineItem(
            string description,
            double price,
            int quantity,
            double taxPercentage)
        {
            LineItems.Add(LineItem.CreateLineItem(
                description,
                price,
                quantity,
                taxPercentage));
        }

        public void RemoteLineItem(Guid lineItemUniqueId)
        {
            var matchingLineItemIndex = LineItems.FindIndex(x => x.UniqueId == lineItemUniqueId);
            if (matchingLineItemIndex != -1)
            {
                LineItems.RemoveAt(matchingLineItemIndex);
            }
        }

        public void UpdateLineItem(
            Guid lineItemUniqueId,
            UnitDescription? description,
            UnitPrice? unitPrice,
            UnitQuantity? unitQuantity,
            Tax? tax)
        {
            if (Status != Status.Draft)
            {
                throw new Exception("A line item can only be edited when an invoice is in draft.");
            }

            var matchingLineItem = LineItems.Find(li => li.UniqueId == lineItemUniqueId);
            if (matchingLineItem == null)
            {
                throw new Exception($"Line item with ID {lineItemUniqueId} could not be found on the invoice.");
            }

            if (description != null)
            {
                matchingLineItem.Description = description;
            }

            if (unitPrice != null)
            {
                matchingLineItem.Price = unitPrice;
            }

            if (unitQuantity != null)
            {
                matchingLineItem.Quantity = unitQuantity;
            }

            if (tax != null)
            {
                matchingLineItem.Tax = tax;
            }
        }

        public void SendInvoice()
        {
            if (Status == Status.Draft)
            {
                if (LineItems.Count == 0)
                {
                    throw new Exception("You need at least one line item in an invoice.");
                }
                else if (PaymentDeadline < DateTime.UtcNow)
                {
                    throw new Exception("A payment deadline in the future must be selected.");
                }

                IssueDate = DateTime.UtcNow;
                Status = Status.Sent;
            }
            else
            {
                throw new Exception("You can only send invoices that are in draft.");
            }
        }

        public void CancelInvoice(string cancellationReason)
        {
            if (Status == Status.Draft)
            {
                throw new Exception("You can only cancel invoices that have been sent.");
            }
            else if (Status == Status.Paid || AmountPaid != 0d)
            {
                throw new Exception("You can only cancel invoices that have not been paid or partially paid.");
            }
            else if (string.IsNullOrEmpty(cancellationReason))
            {
                throw new ArgumentException("You can only cancel invoices with a cancellation reason.");
            }
            else
            {
                Status = Status.Cancelled;
                CancellationReason = cancellationReason;
            }
        }
    }
}
