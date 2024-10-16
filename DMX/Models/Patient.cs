using DMX.Data;
using DMX.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX
{
    public class Patient : TableAudit
    { [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string FolderNo { get; set; }= string.Empty;
        public string FinalDiagnoses { get; set; } = string.Empty;
        public string Depositor { get; set; } = string.Empty;
        public string DepositorAddress { get; set; } = string.Empty;
        [ForeignKey(nameof(DeceasedType.DeceasedTypeId))]
        public DeceasedType DeceasedType { get; set; }
     public string DeceasedTypeId { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public string TagNo { get; set; }=string.Empty;
        public string WardInCharge { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString("N").Substring(0,5);
    public virtual ICollection<PatientComment>PatientComments { get; set; } 
    }
}
