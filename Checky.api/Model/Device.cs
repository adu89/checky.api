using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Device
    {
        //Identity
        public int DeviceId { get; set; }
        public string DeviceGuid { get; set; }

        //Link
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        //Properties
        public string DeviceName { get; set; }
        public bool Deleted { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
