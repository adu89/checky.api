using System;
namespace Checky.api.View
{
    public class InventoryItem
    {
        public string InventoryItemGuid { get; set; }
        public Item Item { get; set; }
        public string Quantity { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
