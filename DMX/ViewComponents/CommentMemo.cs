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
            Random random = new();
            string colorCode = String.Format("#{0:X6}", random.Next(0x1000000));
            return colorCode;
        }
        //public static string GetUserColor(string userId)
        //{
        //    Random random = new();
        //    var colors  = String.Format("#{0:X6}", random.Next(0x1000000));
            

        //    var index = Math.Abs(userId.GetHashCode()) % colors.Length;
        //    return colors[index];
        //}
        public IViewComponentResult Invoke(string Id)
        {

           // string userColor = HttpContext.Session.GetString("UserColor");

            //if (string.IsNullOrEmpty(userColor))
            //{
                //userColor = GenerateRandomColor();
                //HttpContext.Session.SetString("UserColor", userColor);
            

            Memo memoToComment = new();
            memoToComment = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)) where m.MemoId == @Encryption.Decrypt(Id )select m).FirstOrDefault();
            //MemoComment memoToComment=(from m in dcx.MemoComments where m.Memo.MemoId==@Encryption.Decrypt(Id) select m).FirstOrDefault();

            MemoCommentVM addCommentVM = new()
            {
                MemoContent = memoToComment.Content,
                
                Comments=memoToComment.MemoComments.OrderBy(m => m.CreatedDate).ToList(),
                Title = memoToComment.Title,
           
                
            };
            

            return View(addCommentVM);
        }
    }
}

