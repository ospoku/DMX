using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;

namespace DMX.ViewComponents
{
    public class DetailExcuseDuty(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {
            var decodedId = System.Web.HttpUtility.UrlDecode(Id)?.Replace(" ", "+"); // sanitize
            var decryptedId = Encryption.Decrypt(decodedId);
            if (!Guid.TryParse(decryptedId, out Guid dutyGuid))
            {
                return View("Error", "Invalid Excuse Duty Id format");
            }

            ExcuseDuty excuseDutyDetail = new();
            excuseDutyDetail = (from a in dcx.ExcuseDuties where a.PublicId == dutyGuid & a.IsDeleted == false select a).FirstOrDefault();
            DetailExcuseDutyVM excuseDutyVM = new ()
            {
               
                DateofDischarge= excuseDutyDetail.DateofDischarge,
                ExcuseDays=new ExcuseDuty().ExcuseDays,
             
                Diagnosis = new ExcuseDuty().  Diagnosis,
               SelectedUsers = (from x in dcx.ExcuseDutyAssignments where x.PublicId == dutyGuid select x.UserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
            };
            return View(excuseDutyVM);
        }
    }
}
