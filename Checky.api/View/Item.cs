using System;
namespace Checky.api.View
{
    public class Item
    {
        public string ItemGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Weight { get; set; }
        public string Calories { get; set; }
        public Vendor Vendor { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
