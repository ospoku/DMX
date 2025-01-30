using DMX.Data;
using DMX.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX
{
    public class Deceased : TableAudit
    { [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string? DeceasedId { get; set; }
        public string Name { get; set; } 
        public  string FolderNo { get; set; }
        public  string Diagnoses { get; set; } 
        public  string Depositor { get; set; } 
        public string DepositorAddress { get; set; } 
        [ForeignKey(nameof(DeceasedType.DeceasedTypeId))]
        public DeceasedType DeceasedType { get; set; }
         public string DeceasedTypeId { get; set; } 
        public  string? Description { get; set; }
        public string TagNo { get; set; }
        public string WardInCharge { get; set; }
       
        public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString("N").Substring(0,5);
    public virtual ICollection<DeceasedComment>DeceasedComments { get; set; } 
       

  
    }
}
