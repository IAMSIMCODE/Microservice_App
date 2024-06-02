using Microsoft.AspNetCore.Identity;

namespace SimCode.Services.AuthAPI.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
