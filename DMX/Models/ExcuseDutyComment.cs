using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ExcuseDutyComment:TableAudit
    {
        [Key]
        
        public int Id { get; set; }
      public Guid CommentId { get; set; }
        public string Message { get; set; }
        public Guid ExcuseDutyId { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(ExcuseDutyId))]
               public ExcuseDuty ExcuseDuty { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }
    }
}
