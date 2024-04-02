using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class Assignment : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AssignmentId { get; set; }
        public string TaskId { get; set; }
        public string SelectedUsers { get; set; }
        
        public bool IsRead { get; set; } = false;

    }
}
