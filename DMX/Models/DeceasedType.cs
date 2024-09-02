using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace DMX.Models
{
    public class DeceasedType:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DeceasedTypeId { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
