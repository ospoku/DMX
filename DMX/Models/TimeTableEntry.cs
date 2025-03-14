using DMX.Data;
using DMX.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX
{
    public class TimeTableEntry : TableAudit
    {
  
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int TimeTableEntryId { get; set; }

            // Foreign key for Group
            public int GroupId { get; set; }

            // Navigation property for Group
            public Group Group { get; set; }

            // Foreign key for Teacher
            public int TeacherId { get; set; }

            // Navigation property for Teacher
            public Teacher Teacher { get; set; }

            // Foreign key for Classroom
            public int ClassroomId { get; set; }

            // Navigation property for Classroom
            public Classroom Classroom { get; set; }

            // Foreign key for TimeSlot
            public int TimeSlotId { get; set; }

            // Navigation property for TimeSlot
            public TimeSlot TimeSlot { get; set; }

            public DayOfWeek Day { get; set; } // e.g., Monday, Tuesday
        }
    } 