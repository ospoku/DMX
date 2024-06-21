using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class ViewMemos(XContext dContext, UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        private readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            string user = usm.GetUserAsync(HttpContext.User).Result.UserName;
            var memoList = dcx.MemoAssignments.Where(a=>a.AppUser.UserName==user).Select(a => new ViewMemosVM
            {
                MemoId = a.Memo.MemoId,

                Content = a.Memo.Content,
                ReferenceNumber=a.Memo.ReferenceId,
             
                Recipient = a.Memo.Recipient,
                Title = a.Memo.Title,
                Sender = user,
                CreatedDate = a.CreatedDate,


            }).OrderByDescending(a=>a.CreatedDate).ToList();
            return View(memoList);
        }
    }
}
