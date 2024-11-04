using DMX.Data;

namespace DMX.Models
{
    public class Status:TableAudit
    { 
        public string Id { get; set; }
        public required string Name { get; set; }

    }
}
