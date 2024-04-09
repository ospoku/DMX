using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ExcuseDutyComment:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
      
        public string Message { get; set; }
        public string ExcuseDutyId { get; set; }
        public string UserId { get; set; }
        public ExcuseDuty ExcuseDuty { get; set; }
        public AppUser AppUser { get; set; }
    }
}
