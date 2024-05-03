using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddPatient(UserManager<AppUser> userManager, XContext xContext) : ViewComponent
    {
        
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = xContext;
        public IViewComponentResult Invoke()
        {
            AddMorgueVM addPatientVM = new()
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
                DeceasedTypes= new SelectList(dcx.DeceasedTypes.ToList(), "Id","Name")
                
            };
            return View(addPatientVM);
        }
    }
}
