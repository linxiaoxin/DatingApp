using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Model
{
    public class UserRole: IdentityUserRole<int>
    {
        public User user { get; set; }
        public Role role { get; set; }
    }
}