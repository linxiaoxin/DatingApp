using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Model
{
    public class Role: IdentityRole<int>
    {
        public ICollection<UserRole> userRoles { get; set; }

    }
}