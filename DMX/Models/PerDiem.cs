using DMX.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class PerDiem:TableAudit
    {
        public PerDiem()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PerDiemId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public string DepartmentId { get; set; }  
        public string Rank { get; set; }
        public string UserId { get; set; }
        [Precision(10,4)]
        public decimal Amount { get; set;   }
    }
}
