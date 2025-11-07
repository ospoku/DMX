using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class DeceasedAssignment : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid AssignmentId { get; set; }
        public required Guid DeceasedId { get; set; }
        [ForeignKey(nameof(DeceasedId))]
        public Deceased Deceased { get; set; }
        public required string UserId { get; set; }
        [ForeignKey(nameof( UserId))]
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
