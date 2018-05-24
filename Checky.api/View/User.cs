using System;
using System.Collections.Generic;

namespace Checky.api.View
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Pin { get; set; }
        public string Birthdate { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public Organization Organization { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
