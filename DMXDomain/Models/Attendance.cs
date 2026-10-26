using System.ComponentModel.DataAnnotations;

namespace DMXDomain.Models
{
    public class MeetingAttendance : TableAudit
    {
        [Key]
        public int Id { get; set; }
        public Guid AttendanceId { get; set; } = Guid.NewGuid();
        public string ParticipantId { get;  set; }
        
        public string EventId { get; set; }
      
    }
}
