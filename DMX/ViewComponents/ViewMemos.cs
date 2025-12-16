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
    public class ViewMemos : ViewComponent
    {
        private readonly XContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthorizationService _authorization;

        public ViewMemos(
            XContext context,
            UserManager<AppUser> userManager,
            IAuthorizationService authorization)
        {
            _context = context;
            _userManager = userManager;
            _authorization = authorization;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var memoAssignments = await _context.MemoAssignments
                .Include(a => a.Memo)
                .Include(a => a.AppUser)
                .Where(a =>
                    a.AppUser.Id == currentUser.Id ||
                    (a.Memo.CreatedBy == currentUser.Id && !a.Memo.IsDeleted))
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

            var viewModel = new List<ViewMemosVM>();

            foreach (var assignment in memoAssignments)
            {
                var authorizationResult = await _authorization.AuthorizeAsync(
                    HttpContext.User,
                    assignment.Memo,
                    "MemoOwnerPolicy");

                var sender = await _userManager.FindByIdAsync(assignment.Memo.CreatedBy);

                viewModel.Add(new ViewMemosVM
                {
                    PublicId = assignment.Memo.PublicId,
                    Title = assignment.Memo.Title,
                    Content = assignment.Memo.Content,
                    ReferenceNumber = assignment.Memo.ReferenceId,
                    CreatedDate = assignment.CreatedDate,
                    CreatedBy = assignment.Memo.CreatedBy,
                    Sender = sender?.Fullname,
                    Assignees = memoAssignments
                        .Where(x => x.MemoId == assignment.MemoId)
                        .Select(x => x.AppUser.UserName)
                        .Distinct()
                        .ToList(),
                    CanEdit = authorizationResult.Succeeded
                    
                });
            }

            return View(viewModel);
        }
    }
}
