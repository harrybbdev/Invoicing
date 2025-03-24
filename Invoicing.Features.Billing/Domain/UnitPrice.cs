﻿namespace Invoicing.Features.Billing.Domain
{
    public class UnitPrice
    {
        public static UnitPrice CreateUnitPrice(double unitPrice)
        {
            if (unitPrice <= 0)
            {
                throw new ArgumentException("Unit price must have a positive value");
            }

            return new UnitPrice(unitPrice);
        }

        public double Value { get; }

        private UnitPrice(double unitPrice)
        {
            Value = unitPrice;
        }

    }
}
