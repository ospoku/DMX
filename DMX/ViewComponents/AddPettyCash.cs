using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddPettyCash(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            AddPettyCashVM addPettyCashVM = new()
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
                Name =usm.FindByNameAsync( HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Fullname,
            };

            return View(addPettyCashVM);
        }
    }
}
