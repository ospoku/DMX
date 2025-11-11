using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DMX.ViewComponents
{
    public class PrintExcuseDuty(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public async  Task<IViewComponentResult> InvokeAsync(string Id)
        { 
            var decodedId=HttpUtility.UrlDecode(Id)?.Replace(" ","+");
            var decryptdId=Encryption.Decrypt(decodedId);
            if(!Guid.TryParse(decryptdId,out Guid dutyGuid))
            {

            }
         var   dutyToPrint = (from m in dcx.ExcuseDuties.Include(m => m.ExcuseDutyComments.OrderBy(m => m.CreatedDate)) where m.PublicId == dutyGuid select m).FirstOrDefault();

            ExcuseDutyCommentVM addCommentVM = new()
            {
                
              Comments=dutyToPrint.ExcuseDutyComments,
               
               // Sender = (await usm.FindByIdAsync(dutyToPrint.CreatedBy)).Fullname,    
            };
            
            return View(addCommentVM);
        }
    }
}
