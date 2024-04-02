using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class ExcuseDuty : TableAudit
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ExcuseFormId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateofDischarge { get; set; }
        public string OperationDiagnosis { get; set; }
        public string ExcuseDays { get; set; }
        public virtual ICollection<AppUser> ApplicationUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
