using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ServiceAssignment : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid AssignmentId { get; set; } = Guid.NewGuid();
        public string ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
