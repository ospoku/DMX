using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.ViewModels;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class AddLetter(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            AddLetterVM addLetterVM = new()
            {

                UsersList = new SelectList(usm.Users.ToList(), "Id","UserName"),


            };

            return View(addLetterVM);
        }

    }
}
