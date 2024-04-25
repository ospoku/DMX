using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class Workshop:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string WorkshopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set;}

    }
}
