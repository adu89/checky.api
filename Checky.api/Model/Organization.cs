using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Organization
    {
        //Indentity
        public int OrganizationId { get; set; }
        public string OrganizationGuid { get; set; }

        //Properties
        public string OrganizationName { get; set; }
        public int MaxDevices { get; set; }

        //Navigation
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Inventory> Inventories { get; set; }
        public IEnumerable<Device> Devices { get; set; }
        public IEnumerable<OrganizationItem> OrganizationItems { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
