using System;

namespace Domain
{
    public class User : BaseEntity
    {
        public Profile Profile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
