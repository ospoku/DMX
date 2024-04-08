using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class EditPettyCashVM
    {
        public string PettyCashId { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Purpose { get; set; }
    }
}
