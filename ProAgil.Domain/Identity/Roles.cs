using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProAgil.Domain.Identity
{
    public class Roles : IdentityRole<int>
    {
        public List<UserRoles> UserRoles { get; set; }
    }
}