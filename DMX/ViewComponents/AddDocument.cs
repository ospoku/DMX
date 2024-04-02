using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.ViewModels;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class AddMaternityLeave(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            AddMaternityLeaveVM addMaternityLeaveVM = new()
            {

                UsersList = new SelectList(usm.Users.ToList(), "Id","UserName"),


            };

            return View(addMaternityLeaveVM);
        }

    }
}
