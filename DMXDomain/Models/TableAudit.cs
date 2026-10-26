using System.ComponentModel.DataAnnotations;

namespace DMXDomain.Models
{
    public class TableAudit
    {

        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; } 
       
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
      
        public DateTime? ModifiedDate { get; set; }
       
    }
}
