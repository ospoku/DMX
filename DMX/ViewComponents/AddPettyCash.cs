using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddPettyCash : ViewComponent
    {
        public readonly UserManager<AppUser> usm;

        public AddPettyCash(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
            AddPettyCashVM addPettyCashVM = new AddPettyCashVM
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName")
            };

            return View(addPettyCashVM);
        }
    }
}
