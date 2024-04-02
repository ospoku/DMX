using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class EditMemoVM
    {
        public string MemoId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

    
        public string[] SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }

        
    }
}
