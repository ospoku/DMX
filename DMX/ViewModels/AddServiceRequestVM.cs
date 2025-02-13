using DMX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddServiceRequestVM
    {


        public string Description { get; set; }
        public string RequestNumber { get; set; }
        public string ServiceRequestedBy { get; set; }
        public DateTime RequestDate { get; set; }

        public string Unit { get; set; }
        public string Faults { get; set; }
        public SelectList RequestTypes {  get; set; }
        public SelectList Status { get; set; }
        public SelectList Urgency { get; set; }
        public List<CheckBoxItem> Categories { get; set; }

        public string FaultInspectedBy { get; set; }

        public string ActionToBeTaken { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
                      
