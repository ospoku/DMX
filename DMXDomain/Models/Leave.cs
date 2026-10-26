namespace DMXDomain.Models
{
    public class Leave : TableAudit
    {
        public string LeaveId { get; set; }
       
    public virtual ICollection<LeaveComment>Comments { get; set; }  
    }
}
