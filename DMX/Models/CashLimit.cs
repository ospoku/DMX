using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class CashLimit:TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CashLimitId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
       
        public decimal Amount { get; set; } = 0;
    }
}
