using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DMXDomain.Models
{
    public class PettyCash : TableAudit
    {
        
        [Key]
        public int Id { get; set; }
    public Guid PublicId { get; set; } = Guid.NewGuid();
        public string ReferenceNumber { get; set; } = string.Concat("P", Guid.NewGuid().ToString("N").AsSpan(0, 5));  
     
        public string Purpose { get; set; }

        [Precision(10, 4)]
        public decimal Amount { get; set; }
      
        public DateTime Date { get; set; }
      
        public virtual ICollection<PettyCashComment> Comments { get; set; } 
    }
}
