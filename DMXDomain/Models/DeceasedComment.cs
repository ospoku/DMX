using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DMXDomain.Models
{
    public class DeceasedComment:TableAudit
    {
        [Key]
     public int DeceasedCommentId { get; set; }
        public Guid PublicId { get; set; }= Guid.NewGuid();
        [Required]
        public string Message { get; set; }
        [ForeignKey(nameof(DeceasedId))]
        [Required]
        public  int DeceasedId { get; set; }
        [Required]
        public string UserId { get; set; }
        public Deceased Deceased { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }
    }
}
