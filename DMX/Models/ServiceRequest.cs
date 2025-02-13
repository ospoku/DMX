using DMX.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMX.Models
{
    public class ServiceRequest:TableAudit
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ServiceRequestId { get; set; }
        public string RequestNumber { get; set; } = "SR" + Guid.NewGuid().ToString("N").Substring(0, 5);

        public string Faults { get; set; }

        public string ActionToBeTaken { get; set; }
       
        
            
         
         

           
            public string Priority { get; set; } // Priority level (Low, Medium, High, Emergency)
            public string Description { get; set; } // Detailed description of the request
            public string Location { get; set; } // Location where service is needed
            public byte[]? Attachments { get; set; } // File paths or links to attachments
       public string ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }
        public string CategoryId { get; set; }
           
        public Category Category { get; set; }
           public Status Status { get; set; }
            public string StatusId { get; set; } // Current status (Pending, In Progress, Completed, Cancelled)
            public DateTime CompletionDate { get; set; } // Date and time completed
            
            public bool IsApproved { get; set; } // Approval status
            public string ApprovedBy { get; set; } // Name or ID of approver
            public DateTime ApprovalDate { get; set; } // Date and time approved

            public decimal EstimatedCost { get; set; } // Estimated cost
            public decimal ActualCost { get; set; } // Actual cost
            public string BillingInfo { get; set; } // Billing details

            public string RequesterFeedback { get; set; } // Feedback from requester
            public string ResolutionQuality { get; set; } // Quality of service provided
            public bool FollowUpRequired { get; set; } // Indicates if follow-up is needed

            public string SLA { get; set; } // Service Level Agreement
            public string EscalationLevel { get; set; } // Escalation level
            public string RequestSource { get; set; } // Source of the request
            public List<int> RelatedRequests { get; set; } // List of related request IDs
        

        public virtual ICollection<ServiceRequestComment>Comments { get; set; }    
       
      
    }
}
