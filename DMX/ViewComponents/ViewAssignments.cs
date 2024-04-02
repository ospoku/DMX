using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewAssignments(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    { public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
          

            Task<AppUser> userId = usm.FindByNameAsync(HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("Name")).Value);

            var stringIds = dcx.Assignments.Where(x => x.SelectedUsers.Contains(userId.Result.Id.ToString())).Select(x => new ViewAssignmentsVM
            {

               TaskId = x.TaskId,

           }).ToList();

      


            return View(stringIds);
        }
    }
}
