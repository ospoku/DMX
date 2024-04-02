using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddLeaveVM
    {
        public string LeavesId { get; set; }
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
