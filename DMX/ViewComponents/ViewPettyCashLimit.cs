using DMX.Data;
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
        public readonly XContext dcx;
        public ViewPettyCashLimit(UserManager<AppUser> userManager,XContext context )
        {
            usm = userManager;
            dcx = context;

        }
        public IViewComponentResult Invoke()
        {
            ViewPettyCashLimitVM viewPettyCashLimit = new()
            {
                Amount = dcx.PettyCashLimits.Select(p => p.PettyCashLimitAmount).FirstOrDefault(),
                LimitId=dcx.PettyCashLimits.Select(p=>p.PettyCashLimitId).FirstOrDefault(),    

            };

            return View(viewPettyCashLimit);
        }
    }
}
