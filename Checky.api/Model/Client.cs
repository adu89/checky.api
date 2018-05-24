using System;
namespace Checky.api.Model
{
    public class Client
    {
        public Identity Identity { get; set; }
        public User User { get; set; }
        public Organization Organization { get; set; }
        public string Role { get; set; }
    }
}
