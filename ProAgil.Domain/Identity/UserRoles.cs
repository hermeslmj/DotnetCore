using Microsoft.AspNetCore.Identity;

namespace ProAgil.Domain.Identity
{
    public class UserRoles : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Roles Roles { get; set; }
    }
}