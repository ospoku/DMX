using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class FeeStructure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int MinDays  {  get; set;} 
        public int MaxDays { get; set; }
        public decimal Fee { get; set; }
    }
}
