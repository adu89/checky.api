using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class Role
    {
        //Identity
        public int RoleId { get; set; }
        public string RoleGuid { get; set; }

        //Properties
        public string RoleName { get; set; }

        //Navigation
        public IEnumerable<UserRole> UserRoles { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
