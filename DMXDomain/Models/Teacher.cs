using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMXDomain.Models
{
    public class Teacher : TableAudit
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }
        public string ReferenceNumber { get; set; } = string.Concat("T", Guid.NewGuid().ToString("N").AsSpan(0, 5));  
     public string FacultyId { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }

      
 
      
   
    }
}
