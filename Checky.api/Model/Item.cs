using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Item
    {
        //Identity
        public int ItemId { get; set; }
        public string ItemGuid { get; set; }

        //Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public double Calories { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        //Navigation
        public IEnumerable<InventoryItem> InventoryItems { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<OrganizationItem> OrganizationsItems { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
