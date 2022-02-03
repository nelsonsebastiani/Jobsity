using Microsoft.AspNetCore.Identity;

namespace JobsityChatAPI.Models
{
    public class LocalUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
