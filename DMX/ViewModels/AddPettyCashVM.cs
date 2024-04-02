using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddPettyCashVM
    {
        public string PettyCashId { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
        public string Description { get; set; }
    }
}
