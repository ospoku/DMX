using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class PettyCashAssignment : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid AssignmentId { get; set; } = Guid.NewGuid();
        public string PettyCashId { get; set; }
        [ForeignKey("PettyCashId")]
        public PettyCash PettyCash { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
