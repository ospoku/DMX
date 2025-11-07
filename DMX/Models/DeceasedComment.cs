using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMX.Data;

namespace DMX.Models
{
    public class DeceasedComment:TableAudit
    {
        [Key]
     public int Id { get; set; }
        public Guid CommentId { get; set; }= Guid.NewGuid();
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
