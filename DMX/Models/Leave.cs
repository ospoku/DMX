using DMX.Data;

namespace DMX.Models
{
    public class Leave : TableAudit
    {
        public string LeaveId { get; set; }
        public virtual ICollection<AppUser> ApplicationUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
