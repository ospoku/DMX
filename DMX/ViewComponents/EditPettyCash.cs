using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class EditPettyCash(XContext dContext, UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
       
        public readonly UserManager<AppUser> usm = userManager;
        public IViewComponentResult Invoke(string Id)


        {
           

     
      var      pettyCashToUpdate = (from p in dcx.PettyCash where p.PettyCashId==@Encryption.Decrypt(Id) select p ).FirstOrDefault();

            EditPettyCashVM editMemoVM = new()
            {
                Amount=pettyCashToUpdate.Amount,
       Date=pettyCashToUpdate.Date,
         
       Purpose=pettyCashToUpdate.Purpose,
       
                SelectedUsers = dcx.PettyCashAssignments.Where(x => x.PettyCashId == @Encryption.Decrypt(Id)).Select(u => u.AppUserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),

            };
            

            return View(editMemoVM);
        }
    }
}
