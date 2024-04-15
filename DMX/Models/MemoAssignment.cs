using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class MemoAssignment : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string MemoId { get; set; }
        public Memo Memo { get; set; }  
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
