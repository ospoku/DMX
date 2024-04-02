using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class MemoComment(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)


        {


            Memo memoToEdit = new();
            memoToEdit = (from m in dcx.Memos.Include(m => m.Comments.OrderBy(m => m.CreatedDate)) where m.MemoId == Id select m).FirstOrDefault();

            MemoCommentVM addCommentVM = new()
            {
                MemoContent = memoToEdit.Content,
                Comments = memoToEdit.Comments,
                Title = memoToEdit.Title,
              

        
                UsersList= new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            

            return View(addCommentVM);
        }
    }
}
