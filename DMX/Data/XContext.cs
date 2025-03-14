using Microsoft.EntityFrameworkCore;
using DMX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.ObjectModelRemoting;



namespace DMX.Data
{
    public class XContext(DbContextOptions<XContext> options) : AuditableIdentityContext(options)
    {
     
     
    
     
     
        public DbSet<Subject> Subjects { get; set; }
     
        public DbSet<TimeTableEntry> TimeTableEntries { get; set; }
        public object TimetableEntries { get; internal set; }
        public DbSet<Classroom> Classrooms { get; set; }
  
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeSlot> TimeSlots { get;  set; }
        public DbSet < Constraint> Constraints { get;  set; }
    }
}
