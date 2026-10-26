using System.ComponentModel.DataAnnotations;

namespace DMXDomain.Models
{
    public class DeceasedType:TableAudit
    {
        [Key]
     public int DeceasedTypeId { get; set; }
        public Guid PublicId { get; set; }
        [Required]
        public   string Name { get; set; }
        [Required]
        public  string Description { get; set; }
        [Required]
        public  string Code { get; set; }
    }
}
