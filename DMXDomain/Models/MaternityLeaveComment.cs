using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMXDomain.Models
{
    public class MaternityLeaveComment:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public string Id { get; set; }
    }
}
