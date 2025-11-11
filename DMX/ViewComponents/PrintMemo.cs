using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DMX.ViewComponents
{
    public class PrintMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public async  Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var decodedId=HttpUtility.UrlDecode(Id)?.Replace(" ","+");
            var decryptedId=Encryption.Decrypt(decodedId);
            if (!Guid.TryParse(decryptedId, out Guid printGuid)) { }

           
         var   memoToEdit = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)) where m.PublicId == printGuid select m).FirstOrDefault();

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
