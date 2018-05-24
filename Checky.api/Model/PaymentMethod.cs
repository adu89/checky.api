using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class PaymentMethod
    {
        //Identity
        public int PaymentMethodId { get; set; }
        public string PaymentMethodGuid { get; set; }

        //Properties
        public string Type { get; set; }
        public string Details { get; set; }
        public bool Default { get; set; }

        //Link
        public int UserId { get; set; }
        public User User { get; set; }

        //Navigation
        public IEnumerable<Transaction> Trancsactions { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
