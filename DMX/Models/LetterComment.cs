using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMX.Data;

namespace DMX.Models
{
    public class LetterComment:TableAudit
    {
        [Key]
      public int id { get; set; }
        public Guid CommentId { get; set; } = Guid.NewGuid();
        public required string Message { get; set; }
        public required string LetterId { get; set; }
        public  string UserId { get; set; }
        public Letter Letter { get; set; }
        public AppUser AppUser { get; set; }
    }
}
