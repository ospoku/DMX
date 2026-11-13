
using DMXDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure.Data
{
    public class XContext(DbContextOptions<XContext> options) : AuditableIdentityContext(options)
    {
        public DbSet<TransportType> TransportTypes { get; set; }
       public DbSet<CashLimit> CashLimits { get; set; }
        public DbSet<FeeStructure> FeeStructures { get; set; }
        public DbSet<MeetingAttendance> MeetingAttendance { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Letter> Letters { get; set; }
        public DbSet<DeceasedService> DeceasedServices { get; set; }
      
     public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<DeceasedType> DeceasedTypes { get; set; }
        public DbSet<MaternityLeave> MaternityLeaves { get; set; }
        public DbSet<Deceased> Deceased { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<PettyCash> PettyCash { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
       
    
        public DbSet<TravelType> TravelTypes { get; set; }
        public DbSet<TravelRequest> TravelRequests { get; set; }
        public DbSet<MorgueService> MorgueServices { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<ExcuseDuty> ExcuseDuties { get; set; }
        public DbSet<SMSTask> SMSTasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<PerDiem> PerDiems { get; set; }
        public DbSet<Department> Departments { get; set; }
    
        public DbSet<Status> Statuses { get; set; }
       public DbSet<Category> Categories { get; set; }
        public DbSet <Priority> Priorities { get; set; }
        public DbSet<Permission> Permissions { get; set; }

     
   
    }
}
