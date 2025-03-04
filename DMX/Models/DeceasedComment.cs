using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMX.Data;

namespace DMX.Models
{
    public class DeceasedComment:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Message { get; set; }
        [ForeignKey(nameof(DeceasedId))]
        [Required]
        public  string DeceasedId { get; set; }
        [Required]
        public string UserId { get; set; }
        public Deceased Deceased { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }
    }
}
