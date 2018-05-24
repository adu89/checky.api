using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorGuid { get; set; }

        public string Name { get; set; }
        public List<Item> Items { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
