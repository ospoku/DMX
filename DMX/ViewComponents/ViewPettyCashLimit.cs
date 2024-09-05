using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class ViewPettyCashLimit : ViewComponent
    {
        public readonly UserManager<AppUser> usm;

        public ViewPettyCashLimit(UserManager<AppUser> userManager)
        {
            usm = userManager;

        }
        public IViewComponentResult Invoke()
        {
           ViewPettyCashLimitVM viewPettyCashLimit = new()
            {
                
               
        };

            return View(viewPettyCashLimit);
        }
    }
}
