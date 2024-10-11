using DMX.Data;
using DMX.DataProtection;
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
        public readonly XContext  dcx;
     

        public ViewPettyCashLimit(UserManager<AppUser> userManager, XContext context)
        {
            usm = userManager;
            dcx = context;
            
        }
        public IViewComponentResult Invoke()
        {
            //PettyCashLimit limit = new PettyCashLimit();

            //var limitToUpdate = dcx.PettyCashLimits.FirstOrDefault(p => p.PettyCashLimitId ==limit.PettyCashLimitId).PettyCashLimitId;

            ViewPettyCashLimitVM viewPettyCashLimit = new()
            {
                //Amount=limitToUpdate,
               
        };

            return View(viewPettyCashLimit);
        }
    }
}
