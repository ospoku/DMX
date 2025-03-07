using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class MeetingAttendance(UserManager<AppUser> userManager, XContext xContext) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx= xContext;
        public IViewComponentResult Invoke(string Id)
        {


            MeetingAttendanceVM addAttendanceVM = new() 
            {
                EventName= dcx.Meetings.Where(x=>x.MeetingId==Encryption.Decrypt(Id)).Select(x=>x.Name).Single(),

                Attendees = new SelectList(usm.Users.ToList(), "Id", "Fullname"),

            };
            return View(addAttendanceVM);
        }
    }
}
