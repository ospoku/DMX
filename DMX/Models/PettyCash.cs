using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;
using Microsoft.EntityFrameworkCore;

namespace DMX.Models
{
    public class PettyCash : TableAudit
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PettyCashId { get; set; }
        public string ReferenceNumber { get; set; } = "P" + Guid.NewGuid().ToString("N").Substring(0, 5);  
     
        public string Purpose { get; set; }
        [Precision(10, 4)]
        public decimal Amount { get; set; }
      
        public DateTime Date { get; set; }
      
        public virtual ICollection<PettyCashComment> Comments { get; set; } 
    }
}
