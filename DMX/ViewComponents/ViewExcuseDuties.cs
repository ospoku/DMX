using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class ViewExcuseDuties(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = (await usm.GetUserAsync(HttpContext.User)).Id;
            var iList = dcx.ExcuseDutyAssignments.Where(a => a.AppUser.UserName == user || a.ExcuseDuty.CreatedBy == user & a.ExcuseDuty.IsDeleted == false).Select( a => new ViewExcuseDutiesVM
            {

               
                ExcuseDutyId = a.Id,
                Sender=a.ExcuseDuty.CreatedBy,
                DateofDischarge = a.ExcuseDuty.DateofDischarge,
                ExcuseDays = a.ExcuseDuty.ExcuseDays,
                OperationDiagnosis = a.ExcuseDuty.OperationDiagnosis,
                CreatedDate=a.CreatedDate,
    }).OrderByDescending(t=>t.CreatedDate).ToList();
            return View(iList);
        }
    }
}
