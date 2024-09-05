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
        public DateTime DepartureDate { get; set; }
        [Precision(10, 4)]
        public decimal TransportExpenses { get; set; }
        public int NightAbsent { get; set; }
        public DateTime DateofReturn { get; set; }
       
        public string FuelClaim { get; set; }
        [Precision(10, 4)]
        public decimal AmountDue { get; set; }
        public string Purpose { get; set; }
    
      public ModeOfTransport ModeOfTransport { get; set; }
        public string ModeId { get; set; }
        public virtual ICollection<TravelRequestComment>Comments { get; set; }  
       
    }
}
