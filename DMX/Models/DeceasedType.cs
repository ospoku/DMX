using DMX.Data;

namespace DMX.Models
{
    public class DeceasedType:TableAudit
    {
        public string Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
