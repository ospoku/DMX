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
