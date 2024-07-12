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
                Name = a.CreatedBy,
                Date = a.Date,
                Description = a.Description,
                Purpose = a.Purpose,
                ReferenceNumber=a.ReferenceNumber,
                CreatedDate = a.CreatedDate,
            }).OrderByDescending(a => a.CreatedDate).ToList();
            return View(pcList);
        }
    }
}
