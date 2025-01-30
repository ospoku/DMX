using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ExcuseDutyAssignment : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ExcuseDutyId { get; set; }
        [ForeignKey("ExcuseDutyId")]
        public ExcuseDuty ExcuseDuty { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
