using Microsoft.EntityFrameworkCore;
using DMX.Models;

namespace DMX.Data
{
    public class XContext(DbContextOptions<XContext> options) : AuditableIdentityContext(options)
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<Document> Documents { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<MaternityLeave> MaternityLeaves { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<PettyCash> PettyCashes { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<TravelRequest> TravelRequests { get; set; }
        public DbSet<SickReport> SickReports { get; set; }
        public DbSet<ExcuseDuty> ExcuseDuties { get; set; }
        public DbSet<SMSTask> SMSTasks { get; set; }
    }
}
