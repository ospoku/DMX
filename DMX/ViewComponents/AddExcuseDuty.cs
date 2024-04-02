using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class AddExcuseDuty :ViewComponent
    {
        public readonly UserManager<AppUser> usm;

        public AddExcuseDuty(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
            AddExcuseDutyVM addExcuseDutyVM = new AddExcuseDutyVM
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName")
            };
            return View(addExcuseDutyVM);
        }
    }
}
