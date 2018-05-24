using System;
namespace Checky.api.View
{
    public class Organization
    {
        public string OrganizationGuid { get; set; }
        public string OrganizationName { get; set; }
        public string MaxDevices { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
