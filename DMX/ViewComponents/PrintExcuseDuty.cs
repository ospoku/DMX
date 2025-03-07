using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class PrintExcuseDuty(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public async  Task<IViewComponentResult> InvokeAsync(string Id)
        { 
         var   dutyToPrint = (from m in dcx.ExcuseDuties.Include(m => m.ExcuseDutyComments.OrderBy(m => m.CreatedDate)) where m.Id == @Encryption.Decrypt(Id) select m).FirstOrDefault();

            ExcuseDutyCommentVM addCommentVM = new()
            {
                
              Comments=dutyToPrint.ExcuseDutyComments,
               
               // Sender = (await usm.FindByIdAsync(dutyToPrint.CreatedBy)).Fullname,    
            };
            
            return View(addCommentVM);
        }
    }
}
