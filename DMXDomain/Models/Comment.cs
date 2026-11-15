using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMXDomain.Models
{
    public class Comment : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public string Message { get; set; }
        public int ModuleId {  get; set; } 
        public Module Module { get; set; }

       
    }
}
