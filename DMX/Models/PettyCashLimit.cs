using DMX.Data;

namespace DMX.Models
{
    public class PettyCashLimit:TableAudit
    {
        public int Id { get; set; }
        public decimal CashLimit { get; set; }
    }
}
