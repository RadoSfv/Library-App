using Microsoft.AspNetCore.Identity;

namespace Library.Domain
{
    public class LibraryUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}
