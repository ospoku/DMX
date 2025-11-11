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
    public class EditPettyCash(XContext dContext, UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
       
        public readonly UserManager<AppUser> usm = userManager;
        public IViewComponentResult Invoke(string Id)
        {
            var decodedId=HttpUtility.UrlDecode(Id).Replace(" ","+");
            var decryptedId=Encryption.Decrypt(decodedId);
            if (!Guid.TryParse(decryptedId, out Guid cashGuid)) { }
          
      PettyCash    pettyCashToUpdate = (from p in dcx.PettyCash where p.PublicId==cashGuid select p ).FirstOrDefault();

            EditPettyCashVM editPettyCashVM = new()
            {
                Amount=pettyCashToUpdate.Amount,
       Date=pettyCashToUpdate.Date,
       Purpose=pettyCashToUpdate.Purpose,
                SelectedUsers = dcx.PettyCashAssignments.Where(x => x.PublicId == cashGuid).Select(u => u.UserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
                Maximum = dcx.CashLimits.Select(p => p.Amount).FirstOrDefault()
            };
           
            return View(editPettyCashVM);
        }
    }
}
