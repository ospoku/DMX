using Microsoft.AspNetCore.Identity;

namespace DMX.Models
{
    public class AppRole : IdentityRole
    {
        public string Rolename { get; set; }
    }
}
