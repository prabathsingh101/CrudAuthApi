using Microsoft.AspNetCore.Identity;

namespace Auth.API.Models.Domain
{
    public class ApplicationUser: IdentityUser
    {
        public string? Name { get; set; }
    }
}
