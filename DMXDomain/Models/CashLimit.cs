using System.ComponentModel.DataAnnotations;

namespace DMXDomain.Models
{
    public class CashLimit:TableAudit
    {
        [Key]
        public int Id { get; set; }
        public string CashLimitId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
       
        public decimal Amount { get; set; } = 0;
    }
}
