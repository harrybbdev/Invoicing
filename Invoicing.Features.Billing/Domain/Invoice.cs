using Invoicing.Core.Domain;

namespace Invoicing.Features.Billing.Domain
{
    public class Invoice : AggregateRoot
    {
        public static Invoice CreateInvoice(
            DateTime paymentDeadline,
            Currency currency,
            Customer customer)
        {
            return new Invoice(
                Guid.NewGuid(),
                DateTime.MinValue,
                paymentDeadline,
                [],
                Status.Draft,
                currency,
                customer);
        }

        public DateTime IssueDate { get; private set; }
        public DateTime PaymentDeadline { get; private set; }
        public List<LineItem> LineItems { get; private set; }
        public Status Status { get; private set; }
        public Currency Currency { get; private set; }

        public Customer Customer { get; private set; }

        public double Total => LineItems.Sum(x => x.Total);
        public double AmountPaid { get; set; }
        public double AmountOutstanding => Total - AmountPaid;

        private Invoice(
            Guid uniqueId,
            DateTime issueDate,
            DateTime paymentDeadline,
            List<LineItem> lineItems,
            Status status,
            Currency currency,
            Customer customer) : base(uniqueId)
        {
            IssueDate = issueDate;
            PaymentDeadline = paymentDeadline;
            LineItems = lineItems;
            Status = status;
            Currency = currency;
            Customer = customer;
            Status = status;
            Currency = currency;
            Customer = customer;
        }

        public void Update(
            DateTime? paymentDeadline,
            Currency? currency,
            Customer? customer)
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

            if (customer != null)
            {
                Customer = customer;
            }
        }

        public void AddLineItem(
            UnitDescription description,
            UnitPrice unitPrice,
            UnitQuantity unitQuantity,
            Tax tax)
        {
            LineItems.Add(new LineItem(description, unitPrice, unitQuantity, tax));
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

            var matchingLineItem = this.LineItems.Find(li => li.UniqueId == lineItemUniqueId);
            if (matchingLineItem == null)
            {
                throw new Exception($"Line item with ID {lineItemUniqueId} could not be found on the invoice.")
            }

            if (description != null)
            {
                matchingLineItem.Description = description;
            }

            if (unitPrice != null)
            {
                matchingLineItem.UnitPrice = unitPrice;
            }

            if (unitQuantity != null)
            {
                matchingLineItem.UnitQuantity = unitQuantity;
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
                Status = Status.Sent;
            }
            else
            {
                throw new Exception("You can only send invoices that are in draft.");
            }
        }
    }
}
