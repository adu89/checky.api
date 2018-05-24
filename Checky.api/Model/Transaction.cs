using System;
namespace Checky.api.Model
{
    public class Transaction
    {
        //Identity
        public int TransactionId { get; set; }
        public string TransactionGuid { get; set; }

        public double Total { get; set; }
        public string TransactionToken { get; set; }

        //Link
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
