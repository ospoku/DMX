using DMXInfrastructure.Data;
using DMX.ViewModels;
using DMXDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static DMXApplication.Constants.Permissions;


namespace DMX.ViewComponents
{
    public class ViewMemos(
        XContext context,
        UserManager<AppUser> userManager,
        IAuthorizationService authorization) : ViewComponent
    {
        private readonly XContext ctx = context;
        private readonly UserManager<AppUser> usm = userManager;
        private readonly IAuthorizationService auth = authorization;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await usm.GetUserAsync(HttpContext.User);

            var memoAssignments = await ctx.Assignments
              
              
                .Where(a =>
                    a.AssignedTo == currentUser.Id ||
                    (a.CreatedBy == currentUser.Id && !a.IsDeleted))
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

            var viewModel = new List<ViewMemosVM>();

            foreach (var assignment in memoAssignments)
            {
                var authorizationResult = await auth.AuthorizeAsync(
                    HttpContext.User,
                    assignment.AssignedTo,
                    "MemoOwnerPolicy");
                var canPrint = HttpContext.User.HasClaim("Permission", "Permission.Print.Memo");
                var sender = await usm.FindByIdAsync(assignment.CreatedBy);

                viewModel.Add(new ViewMemosVM
                {
                    PublicId = assignment.PublicId,
                    //Title = assignment.Title,
                    //Content = assignment.Memo.Content,
                    
                    CreatedDate = assignment.CreatedDate,
                    CreatedBy = assignment.CreatedBy,
                    Sender = sender?.Fullname,
                    Assignees = ctx.Assignments.Where(x => x.Id == assignment.Id).Select(x => x.AssignedTo).Distinct().ToList(),
                    CanEdit = authorizationResult.Succeeded,
                    CanPrint = canPrint

                });
            }

            return View(viewModel);
        }
    }
}
