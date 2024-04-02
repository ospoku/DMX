using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddPatient :ViewComponent
    {
        
        public readonly UserManager<AppUser> usm;

        public AddPatient(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
            AddPatientVM addPatientVM = new AddPatientVM
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName")
            };
            return View(addPatientVM);
        }
    }
}
