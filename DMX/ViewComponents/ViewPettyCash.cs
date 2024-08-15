using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;
namespace DMX.ViewComponents
{
    public class ViewPettyCash(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public IViewComponentResult Invoke()
        {
            var user = usm.GetUserAsync(HttpContext.User).Result?.UserName;
            var pettyList = dcx.PettyCashAssignments.Where(a => a.AppUser.UserName == user || a.CreatedBy == user).Select(a => 
             new ViewPettyCashVM
            {
                PettyCashId = a.PettyCashId,
                Amount = a.PettyCash.Amount,

                Date = a.PettyCash.Date,

                Purpose = a.PettyCash.Purpose,
                ReferenceNumber=a.PettyCash.ReferenceNumber,
                CreatedDate = a.CreatedDate,
            }).OrderByDescending(a => a.CreatedDate).ToList();
            return View(pettyList);
        }
    }
}
