using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class EditExcuseDutyVM 
    {
        
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateofDischarge { get; set; }
        public string OperationDiagnosis { get; set; }
        public string ExcuseDays { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
