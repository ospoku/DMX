using DMX.Data;

namespace DMX.Models
{
    public class Attendance : TableAudit
    {
        public object ParticipantId { get; internal set; }
        public bool IsPresent { get; internal set; }
        public string TrainingId { get; internal set; }
    }
}
