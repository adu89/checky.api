using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checky.api.Model
{
    public class OrganizationItem
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganizationItemId { get; set; }
        public string OrganizationItemGuid { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
