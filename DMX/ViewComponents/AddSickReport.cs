using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.Models
{
    public class AddSickReport:ViewComponent
    {
        public readonly UserManager<AppUser> usm;

        public AddSickReport(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
            AddSickReportVM addSickReportVM = new AddSickReportVM()
            {
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))))
            };
            return View(addSickReportVM);
        }
    }
}
