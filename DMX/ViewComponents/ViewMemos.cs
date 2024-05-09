using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewMemos(XContext dContext, IHttpContextAccessor contextAccessor) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm;
        private readonly HttpContextAccessor accessor = (HttpContextAccessor)contextAccessor;

        public IViewComponentResult Invoke()
        {
            string user = accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Name").Value;
            var memoList = dcx.Memos.Where(a => a.IsDeleted == false & a.CreatedBy == user).Select(a => new ViewMemosVM
            {
                MemoId = a.MemoId,

                Content = a.Content,

             
                Recipient = a.Recipient,
                Title = a.Title,
                Sender = user,


            }).ToList();
            return View(memoList);
        }
    }
}
