using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddAttendance(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {


            AddAttendanceVM addAttendanceVM = new() 
            {

                Attendees = new SelectList(usm.Users.ToList(), "Id", "Fullname"),

            };
            return View(addAttendanceVM);
        }
    }
}
