using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.ViewModels;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class AddLeave : ViewComponent
    {
        public readonly UserManager<AppUser> usm;
  
        public AddLeave(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }

        public IViewComponentResult Invoke()
        {
            AddLeaveVM addLeaveVM = new AddLeaveVM
            {

                UsersList = new SelectList(usm.Users.ToList(), "Id","UserName"),


            };

            return View(addLeaveVM);
        }
            
    }
}
