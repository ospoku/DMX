using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddServiceRequest:ViewComponent
    {
        public readonly UserManager<AppUser> usm;

        public AddServiceRequest(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
            AddServiceRequestVM addServiceRequest = new AddServiceRequestVM
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName")
            };
            return View(addServiceRequest);
        }
    }
}
