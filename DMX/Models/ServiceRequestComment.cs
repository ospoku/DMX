using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMX.Data;

namespace DMX.Models
{
    public class ServiceRequestComment:TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid CommentId { get; set; } = Guid.NewGuid();

        public string Message { get; set; }
        public string ServiceRequestId { get; set; }
        public string UserId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public AppUser AppUser { get; set; }
    }
}
