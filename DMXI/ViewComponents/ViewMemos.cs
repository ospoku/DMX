using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class ViewMemos(XContext dContext, IHttpContextAccessor contextAccessor) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        
        private readonly HttpContextAccessor accessor = (HttpContextAccessor)contextAccessor;

        public IViewComponentResult Invoke()
        {
            string user = accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Name").Value;
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
