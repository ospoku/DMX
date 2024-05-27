using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class EditPatientVM
    {
        public string FormId { get; set; }
        public string DocumentName { get; set; }
        public string FinalDiagnoses { get; set; }
        public string WardInCharge { get; set; }
        public DateTime Date { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
