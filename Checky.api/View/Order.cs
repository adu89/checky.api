using System;
using System.Collections.Generic;
using Checky.api.Model;

namespace Checky.api.View
{
    public class Order
    {
        public string OrderGuid { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public User User { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
