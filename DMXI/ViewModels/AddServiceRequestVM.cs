﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddServiceRequestVM
    {



        public string RequestNumber { get; set; }
        public string ServiceRequestedBy { get; set; }
        public DateTime RequestDate { get; set; }

        public string Unit { get; set; }
        public string Faults { get; set; }

        public string FaultInspectedBy { get; set; }

        public string ActionToBeTaken { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
                      