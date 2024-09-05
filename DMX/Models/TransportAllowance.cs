using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class TransportAllowance:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public decimal Allowance { get; set; }

    }
}
