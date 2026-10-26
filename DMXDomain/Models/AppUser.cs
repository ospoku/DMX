using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMXDomain.Models
{
    public class    AppUser : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        public string Fullname
        {
            get
            {
                return Firstname
                    + "  "
                    + Lastname;
            }
        }


    
        public bool IsDeleted { get; set; }

        [Precision(10, 4)]
        public decimal Rate { get; set; } = 0;

     
        public string? DepartmentId { get; set; }
        public string? RankId { get; set; }
        public byte[]? Picture { get; set; }
    }

    
}
