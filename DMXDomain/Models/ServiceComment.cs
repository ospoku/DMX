using System.ComponentModel.DataAnnotations;

namespace DMXDomain.Models
{
    public class ServiceComment:TableAudit
    {
        [Key]
        public int ServiceCommentId { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();

        public string Message { get; set; }
        public int ServiceRequestId { get; set; }
        public string UserId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public AppUser AppUser { get; set; }
    }
}
