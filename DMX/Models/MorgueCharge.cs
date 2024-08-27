using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class MorgueCharge
    {
        

        
        public MorgueCharge() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public decimal Tier1 {  get; set; } 
        public decimal Tier2 { get; set; }
    }
}
