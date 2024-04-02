using DMX.Data;
using DMX.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX
{
    public class Patient : TableAudit
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PatientId { get; set; }
        public string DocumentName { get; set; }
        public string FinalDiagnoses { get; set; }
        public string WardInCharge { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<AppUser> ApplicationUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
