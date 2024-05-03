using Microsoft.AspNetCore.Identity;

namespace Microsvc.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
