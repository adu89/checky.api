using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checky.api.Model
{
    public class InventoryItem
    {
        //Identity
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryItemId { get; set; }
        public string InventoryItemGuid { get; set; }

        //Navigation
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        //Properties
        public int Quantity { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
