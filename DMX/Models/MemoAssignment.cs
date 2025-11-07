using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class MemoAssignment : TableAudit
    {
        [Key]
<<<<<<< HEAD
        public int Id { get; set; }
        public Guid AssignmentId { get; set; } = Guid.NewGuid();
=======
      public int Id { get; set; }
        public Guid AssignmentId { get; set; }
>>>>>>> 1454a21726b35c461febf31d14b931aa5002a26b
        public Guid MemoId { get; set; }
        [ForeignKey("MemoId")]
        public Memo Memo { get; set; }  
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
