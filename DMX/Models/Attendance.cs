using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class Attendance : TableAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AttendanceId { get; set; }
        public string ParticipantId { get;  set; }
        public bool IsPresent { get; internal set; }
        public string EventId { get; set; }
        public string EventType { get; set; }   
    }
}
