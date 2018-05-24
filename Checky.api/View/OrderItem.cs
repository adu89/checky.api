using System;
namespace Checky.api.View
{
    public class OrderItem
    {
        public string OrderItemGuid { get; set; }
        public View.Item Item { get; set; }
        public string Quantity { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
