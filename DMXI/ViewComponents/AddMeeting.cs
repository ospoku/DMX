using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddMeeting() : ViewComponent
    {
        

        public IViewComponentResult Invoke()
        {
            AddMeetingVM addMeetingVM = new()
            {
            };
            
        

            return View(addMeetingVM);
        }
    }
}
