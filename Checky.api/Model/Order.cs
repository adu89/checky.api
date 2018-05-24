using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Order
    {
        //Identity
        public int OrderId { get; set; }
        public string OrderGuid { get; set; }

        //Links
        public int UserId { get; set; }
        public User User { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        //Navigation
        public List<OrderItem> OrderItems { get; set; }
        public Transaction Transaction { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }

    }
}
