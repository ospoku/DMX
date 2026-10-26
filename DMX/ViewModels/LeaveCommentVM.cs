using Microsoft.AspNetCore.Mvc.Rendering;
using DMXDomain.Models;
using DMX.ViewComponents;
namespace DMX.ViewModels
{
    public class LeaveCommentVM
    {
        public string MemoId { get; set; }
        public string MemoContent { get; set; }
        public string Title { get; set; }

        public string NewComment { get; set; }
    public virtual ICollection<ViewComponents.LeaveComment> Comments { get; set; } 
        public List<string> SelectedUsers { get; set; }
        public SelectList UsersList { get; set; }
    }
}
