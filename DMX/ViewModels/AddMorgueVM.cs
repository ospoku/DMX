using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddPatientVM
    {
        public string PatientId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public string FinalDiagnoses { get; set; }
        public string WardInCharge { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
