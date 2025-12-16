using Microsoft.AspNetCore.Mvc;

using DMX.Data;
using DMX.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DMX.Models;
using Microsoft.AspNetCore.Authorization;

namespace DMX.ViewComponents
{
    public class ViewMemos(XContext dContext, UserManager<AppUser> userManager, IAuthorizationService authorization) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        private readonly UserManager<AppUser> usm = userManager;
        public readonly IAuthorizationService auth = authorization;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = (await usm.GetUserAsync(HttpContext.User));
            var memoList = dcx.MemoAssignments.Where(a => a.AppUser.Id == user.Id || a.Memo.CreatedBy == user.Id & a.Memo.IsDeleted == false).ToList();

            foreach (var memo in memoList)
            {
                var canEdit = await auth.AuthorizeAsync(HttpContext.User, memo, "MemoOwnerPolicy");

                var canPrint = HttpContext.User.HasClaim("Permission", "Permission.MemoPrint");
                var senderUser = await usm.FindByIdAsync(memo.CreatedBy);
                List<ViewMemosVM>memoVm = new ViewMemosVM()
                {
                    PublicId = memo.PublicId,

                    Content = memo.Memo.Content,
                    ReferenceNumber = memo.Memo.ReferenceId,

                    Assignees = (from u in usm.Users where u.Id == memo.UserId select u.UserName).ToList(),
                    Title = memo.Memo.Title,

                    CreatedDate = memo.CreatedDate,
                    CreatedBy = memo.CreatedBy
                };
            

            return View(memoVmm).ToList());
            }
        }
    } }
