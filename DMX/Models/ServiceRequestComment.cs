using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DMX.Models
{
    public class ServiceRequestComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Message { get; set; }
        public string ServiceRequestId { get; set; }
        public string UserId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public AppUser AppUser { get; set; }
    }
}
