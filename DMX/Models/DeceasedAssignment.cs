using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class DeceasedAssignment : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public required string DeceasedId { get; set; }
        [ForeignKey(nameof(DeceasedId))]
        public Deceased Deceased { get; set; }
        public required string UserId { get; set; }
        [ForeignKey(nameof( UserId))]
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
