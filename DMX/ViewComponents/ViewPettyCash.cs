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
            var pcList = dcx.PettyCashes.Where(a => a.IsDeleted == false).Select(a => new ViewPettyCashVM
            {
                PettyCashId = a.PettyCashId,
                Amount = a.Amount,
                Name = usm.FindByNameAsync(a.CreatedBy).Result.Fullname,
                Date = a.Date,
                Description= a.Description,
                Purpose = a.Purpose,
            }).ToList();
            return View(pcList);
        }
    }
}
