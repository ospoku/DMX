using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class Fee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public decimal Tier1 {  get; set; } 
        public decimal Tier2 { get; set; }
        public decimal PCThreshold { get; set; }
    }
}
