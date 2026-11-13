using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DMXDomain.Models
{
    public class Assignment : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; }= Guid.NewGuid();
     

        public int ModuleId { get; set; }

        public int ModuleRecordId { get; set; }

        public string AssignedTo { get; set; }

        public string AssignedBy { get; set; }

        public DateTime AssignedDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string? Remarks { get; set; }

        public Module Module { get; set; }
 
    }

}

