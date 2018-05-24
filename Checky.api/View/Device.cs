using System;
using System.Collections.Generic;

namespace Checky.api.View
{
    public class Device
    {
        public string DeviceGuid { get; set; }
        public string Password { get; set; }
        public Inventory Inventory { get; set; }
        public Organization Organization { get; set; }
        public string DeviceName { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
