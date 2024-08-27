using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class TrainingAttendance(UserManager<AppUser> userManager, XContext xContext) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx= xContext;
        public IViewComponentResult Invoke(string Id)
        {


            TrainingAttendanceVM addAttendanceVM = new() 
            {
                EventName= dcx.Trainings.Where(x=>x.TrainingId==Encryption.Decrypt(Id)).Select(x=>x.EventName).Single(),

                Attendees = new SelectList(usm.Users.ToList(), "Id", "Fullname"),

            };
            return View(addAttendanceVM);
        }
    }
}
