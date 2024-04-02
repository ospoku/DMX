using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class CommentDocument:ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public CommentDocument(XContext dContext, UserManager<AppUser> userManager)
        {
            dcx = dContext;
            usm = userManager;
        }

        public IViewComponentResult Invoke(string Id)


        {




            Document documentToEdit = new Document();
            documentToEdit = (from d in dcx.Documents.Include(d => d.Comments.OrderBy(d => d.CreatedDate)) where d.DocumentId == Id select d).FirstOrDefault();

            DocumentCommentVM addCommentVM = new DocumentCommentVM
            {
                MemoContent = documentToEdit.AdditionalNotes,
                Comments = (from c in dcx.Comments where c.TaskId == documentToEdit.DocumentId select c).ToList(),
                Title = documentToEdit.ReferenceNumber,
                //SelectedUsers = AssignedUsers,

        
                UsersList= new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            

            return View(addCommentVM);
        }
    }
}
