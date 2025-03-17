using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class MeetingAttendance : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AttendanceId { get; set; }
        public string ParticipantId { get;  set; }
        
        public string EventId { get; set; }
      
    }
}
