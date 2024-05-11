using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ServiceRequest:TableAudit
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ServiceRequestId { get; set; }
        public string RequestNumber { get; set; }
     
        public DateTime RequestDate { get; set; }

        public string Unit { get; set; }
        public string Faults { get; set; }

        public string FaultInspectedBy { get; set; }

        public string ActionToBeTaken { get; set; }
     
     public virtual ICollection<ServiceRequestComment>Comments { get; set; }    
        public virtual ICollection<AppUser> ApplicationUsers { get; set; }
      
    }
}
