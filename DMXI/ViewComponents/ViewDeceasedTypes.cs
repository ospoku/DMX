using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewDeceasedTypes(XContext dContext, IHttpContextAccessor contextAccessor) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        
        private readonly HttpContextAccessor accessor = (HttpContextAccessor)contextAccessor;

        public IViewComponentResult Invoke()
        {
            string user = accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Name").Value;
            var memoList = dcx.Memos.Where(a => a.IsDeleted == false & a.CreatedBy == user).Select(a => new ViewMemosVM
            {
                MemoId = a.MemoId,
                Content = a.Content,
                ReferenceNumber=a.ReferenceId,
                Recipient = a.Recipient,
                Title = a.Title,
                Sender = user,
                CreatedDate = a.CreatedDate,
            }).OrderByDescending(a=>a.CreatedDate).ToList();
            return View(memoList);
        }
    }
}
