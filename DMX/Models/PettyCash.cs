using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class PettyCash : TableAudit
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PettyCashId { get; set; }
        public virtual ICollection<AppUser> ApplicationUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public string Purpose { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
