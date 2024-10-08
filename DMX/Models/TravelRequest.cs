using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;
using Microsoft.EntityFrameworkCore;

namespace DMX.Models
{
    public class TravelRequest:TableAudit
    {[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TravelRequestId { get; set; }
        public string ReferenceNumber { get; set; }
      
        [Precision(10, 4)]
        public decimal? ConferenceFee { get; set; }
        public DateTime EndDate { get; set; }   
        public DateTime StartDate { get; set; }
        [Precision(10, 4)]
        public decimal? OtherExpenses { get; set; }
      
        public DateTime DateofReturn { get; set; }
        [Precision(10, 4)]
        public decimal? FuelClaim { get; set; }
       
        
        public string Purpose { get; set; }
    
      public ModeOfTransport ModeOfTransport { get; set; }
        public string ModeId { get; set; }
        public virtual ICollection<TravelRequestComment>Comments { get; set; }  
       
    }
}
