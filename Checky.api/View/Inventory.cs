using System;
using System.Collections.Generic;

namespace Checky.api.View
{
    public class Inventory
    {
        public string InventoryGuid { get; set; }
        public Organization Organization { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
