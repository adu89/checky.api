using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Inventory
    {
        //Identity
        public int InventoryId { get; set; }
        public string InventoryGuid { get; set; }

        //Link
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        //Navigation
        public IEnumerable<Device> Devices { get; set; }
        public IEnumerable<InventoryItem> InventoryItems { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
