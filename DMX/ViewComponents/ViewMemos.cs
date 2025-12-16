using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static DMX.Constants.Permissions;


namespace DMX.ViewComponents
{
    public class ViewMemos(XContext dContext, UserManager<AppUser>userManager, IAuthorizationService authorization ) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        private readonly UserManager<AppUser> usm = userManager;
        public readonly IAuthorizationService auth = authorization; 
        public async Task<IViewComponentResult>InvokeAsync()
        {
            var currentUser = (await usm.GetUserAsync(HttpContext.User));

            var memoList = dcx.MemoAssignments.Include(a => a.Memo).Where(a => a.AppUser.Id == currentUser.Id || a.Memo.CreatedBy == currentUser.Id & a.Memo.IsDeleted == false).ToList();
            var viewModel = new List<ViewMemosVM>
            {
                new() { PublicId = memo.PublicId,
                Content = a.Memo.Content,
                ReferenceNumber = a.Memo.ReferenceId,
                Assignees = (from u in usm.Users where u.Id == a.UserId select u.UserName).ToList(),
                Title = a.Memo.Title,
                CreatedDate = a.CreatedDate,
                CreatedBy = a.CreatedBy,
        CanEdit=auth.AuthorizeAsync(user,a.Memo,"MemoOwnerPolicy"),

            }).OrderByDescending(a=>a.CreatedDate).ToList();
            foreach (var memo in memoList)
            {
                var canEdit = auth.AuthorizeAsync(user, memo, "MemoOwnerPolicy");
                var senderUser = await usm.FindByIdAsync(memo.CreatedBy);
                memo.Sender = senderUser?.Fullname;
             
            }
            return View(viewModel);
        }
    }
}
