using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMXDomain.Models
{
    public class Category : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public  string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public  string Description { get; set; }

    }
}
