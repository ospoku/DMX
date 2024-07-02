using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class ViewTravelTypes(XContext dContext,UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        private readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            var user = usm.GetUserAsync(HttpContext.User).Result;
            var memoList = dcx.Memos.Where(a => a.IsDeleted == false & a.CreatedBy == user.UserName).Select(a => new ViewMemosVM
            {
                MemoId = a.MemoId,
                Content = a.Content,
                ReferenceNumber=a.ReferenceId,
                Title = a.Title,
                Sender = user.UserName,
                CreatedDate = a.CreatedDate,
            }).OrderByDescending(a => a.CreatedDate).ToList();
            return View(memoList);
        }
    }
}
