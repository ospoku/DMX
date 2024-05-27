using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class EditTravelRequestVM
    {
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
