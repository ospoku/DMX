using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddTravelRequest:ViewComponent
    {
        public readonly UserManager<AppUser> usm;

        public AddTravelRequest(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
            AddTravelRequestVM addTravelRequestVM = new AddTravelRequestVM
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName")
            };

            return View(addTravelRequestVM);
        }
    }
}
