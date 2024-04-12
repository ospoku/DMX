using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class TravelRequest:TableAudit
    {[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TravelRequestId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public string RankId { get; set; }
        public string DepartmentId { get; set; }
        public decimal ConferenceFee { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal TransportExpenses { get; set; }
        public int NightAbsent { get; set; }
        public DateTime DateofReturn { get; set; }
        public int Rate { get; set; }
        public string FuelClaim { get; set; }
        public decimal AmountDue { get; set; }
        public string PurposeofJourney { get; set; }
    
        public virtual ICollection<AppUser> ApplicationUsers { get; set; }
        public virtual ICollection<TravelRequestComment>Comments { get; set; }  
       
    }
}
