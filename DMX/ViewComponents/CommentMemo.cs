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
    public class CommentMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public string GenerateRandomColor()
        {
            Random random = new Random();
            string colorCode = String.Format("#{0:X6}", random.Next(0x1000000));
            return colorCode;
        }
        public IViewComponentResult Invoke(string Id)


        {

            string userColor = HttpContext.Session.GetString("UserColor");

            if (string.IsNullOrEmpty(userColor))
            {
                userColor = GenerateRandomColor();
                HttpContext.Session.SetString("UserColor", userColor);
            }

            Memo memoToEdit = new();
            memoToEdit = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)) where m.MemoId == @Encryption.Decrypt(Id )select m).FirstOrDefault();

            MemoCommentVM addCommentVM = new()
            {
                MemoContent = memoToEdit.Content,
              Comments=memoToEdit.MemoComments,
                Title = memoToEdit.Title,
              
                UsersList= new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            

            return View(addCommentVM);
        }
    }
}

