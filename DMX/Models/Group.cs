using DMX;
using DMX.Models;

public class Group
{

        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public int StudentCount { get; set; } // Number of students in this group

        // Navigation property for TimeTableEntry
        public ICollection<TimeTableEntry> TimeTableEntries { get; set; }
    }



