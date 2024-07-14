using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class    AppUser : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Surname { get; set; }

        public string Fullname
        {
            get
            {
                return Firstname
                    + "  "
                    + Surname;
            }
        }
public string Initials { get {return Fullname.Split(' ').Where(part => !string.IsNullOrEmpty(part))
                               .Select(part => part[0])
                               .Take(2)
                               .ToArray().ToString();
            }
        }

    
        public bool IsDeleted { get; set; }

        [Precision(10,4)]
        public decimal? Rate { get; set; }

     
        public string? DepartmentId { get; set; }
        public string? RankId { get; set; }
    }

    
}
