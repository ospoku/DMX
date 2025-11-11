using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace DMX.ViewComponents
{
    public class EditExcuseDuty(XContext dContext, UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm=userManager;
        public IViewComponentResult Invoke(string Id)


        {
          var decodedId=HttpUtility.UrlDecode(Id)?.Replace(" ","+"); 
            var decryptedId=Encryption.Decrypt(decodedId);
            if(!Guid.TryParse(decryptedId, out Guid dutyGuid))
            {

            }

           ExcuseDuty dutyToUpdate = new();
            dutyToUpdate = (from d in dcx.ExcuseDuties where d.PublicId == dutyGuid select d).FirstOrDefault();

            EditExcuseDutyVM editMemoVM = new()
            {PatientId=dutyToUpdate.PatientId,
            Name=dutyToUpdate.PatientName,
               
               DateofDischarge=dutyToUpdate.DateofDischarge,
               ExcuseDays=dutyToUpdate.ExcuseDays,  
               Diagnosis =dutyToUpdate.Diagnosis,
            
                SelectedUsers = dcx.ExcuseDutyAssignments.Where(x => x.PublicId == dutyGuid).Select(x => x.UserId).ToList(),
                UsersList =  new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
                
            };
            

            return View(editMemoVM);
        }
    }
}
