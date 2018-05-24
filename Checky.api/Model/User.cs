using System;
using System.Collections.Generic;

namespace Checky.api.Model
{
    public class User
    {
        //Identity
        public int UserId { get; set; }
        public string UserGuid { get; set; }

        //Properties
        public string Email { get; set; }
        public string Pin { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset Birthdate { get; set; }

        //Link
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        //Navigation
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        //Dates
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
