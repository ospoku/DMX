using DMX.ViewModels;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddMemo(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            AddMemoVM addMemoVM = new()
            {
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id)),nameof(AppUser.Fullname)),
               
        };

            return View(addMemoVM);
        }
    }
}
