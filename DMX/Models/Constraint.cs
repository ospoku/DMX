namespace DMX.Models
{
    public class Constraint
    {
        public int Id { get; set; }
        public string Type { get; set; } // e.g., "TeacherAvailability", "ClassroomCapacity"
        public string Value { get; set; } // e.g., "TeacherId=1;Day=Monday;IsAvailable=false"
    }
}
