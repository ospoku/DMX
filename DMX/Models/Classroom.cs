using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMX.Data;

namespace DMX.Models
{
    public class Classroom : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ClassroomId { get; set; }
        public string ReferenceId { get; set; }="C"+ Guid.NewGuid().ToString("N").Substring(0,5);
        public string Name { get; set; }
        public int Capacity { get; set; }
        

    
      
    }
}
