using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class PrintMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public async  Task<IViewComponentResult> InvokeAsync(Guid Id)
        {


           
         var   memoToEdit = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)) where m.MemoId == @Encryption.Decrypt(Id) select m).FirstOrDefault();

            MemoCommentVM addCommentVM = new()
            {
                MemoContent = memoToEdit.Content,
              Comments=memoToEdit.MemoComments,
                Title = memoToEdit.Title,
                Sender = (await usm.FindByIdAsync(memoToEdit.CreatedBy)).Fullname,
            
                
            };
            

            return View(addCommentVM);
        }
    }
}
