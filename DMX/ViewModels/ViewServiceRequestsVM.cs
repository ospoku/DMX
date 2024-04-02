namespace DMX.ViewModels
{
    public class ViewServiceRequestsVM
    {
        

        public string ServiceRequestId { get; set; }
            public string RequestNumber { get; set; }
            public string ServiceRequestedBy { get; set; }
            public DateTime RequestDate { get; set; }

            public string Unit { get; set; }
            public string Faults { get; set; }

            public string FaultInspectedBy { get; set; }

            public string ActionToBeTaken { get; set; }
        }
    }




