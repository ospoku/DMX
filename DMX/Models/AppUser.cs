using Microsoft.AspNetCore.Identity;

namespace DMX.Models
{
    public class AppUser:IdentityUser
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }

        public string Fullname
        {
            get
            {
                return Firstname
                    + "  "
                    + Surname; } }


    
        public bool IsDeleted { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Memo> Memos { get; set; }
        public ICollection<PettyCash> PettyCashes { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<MaternityLeave> MaternityLeaves { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<TravelRequest> TravelRequests { get; set; }
        public ICollection<SickReport> SickReports { get; set; }
      public ICollection<Document> Documents { get; set; }


    }

    
}
