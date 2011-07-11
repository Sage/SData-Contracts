using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Integration.Northwind.Adapter.Services
{
    internal class DiscountFactory
    {
        public static decimal DISOUNT_IN_PERCENT = 10;
        public static decimal DISCOUNT_QUANTITY = 10;     // if commodity quantity >= DISCOUNT_QUANTITY allow discount of DISOUNT_IN_PERCENT


        public static Discount GetDiscount(decimal unitPrice, decimal quantity)
        {
            if (quantity < DISCOUNT_QUANTITY)
                return new Discount();

            decimal amount = unitPrice - (unitPrice * (new decimal(1) - DISOUNT_IN_PERCENT / new decimal(100)));
            decimal percent = DISOUNT_IN_PERCENT;

            return new Discount(percent, amount);
        }
    }
}
