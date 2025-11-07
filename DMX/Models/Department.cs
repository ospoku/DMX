using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class Department:TableAudit
    {
        [Key]
public int Id { get; set; }
        public Guid  DepartmentId { get; set; }
        [Required]
        public  string Name { get; set; }
        [Required]
        public  string Code { get; set; }
        
        [Required]
        public  string Description { get; set; }     
    }
}
