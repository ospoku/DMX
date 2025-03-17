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
    public class EditDeceased(UserManager<AppUser> userManager, XContext dContext) :ViewComponent
    {
        public readonly XContext dcx=dContext;

        public readonly UserManager<AppUser> usm = userManager;
        public IViewComponentResult Invoke(string Id)

        {
            
          var  deceasedToEdit = (from m in dcx.Deceased where m.DeceasedId == @Encryption.Decrypt(Id) select m ).FirstOrDefault();

            EditDeceasedVM editDeceasedVM = new EditDeceasedVM
            {
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname)))),
                DeceasedTypes = new SelectList(dcx.DeceasedTypes.ToList(), "DeceasedTypeId", "Code"),
                DeceasedId=deceasedToEdit.DeceasedId,
                Depositor=deceasedToEdit.Depositor,
                DepositorAddress=deceasedToEdit.DepositorAddress,
                Deceased=deceasedToEdit.Name,
                FolderNo=deceasedToEdit.FolderNo,
                TagNo=deceasedToEdit.TagNo,
                Diagnoses=deceasedToEdit.Diagnoses,
                WardInCharge=deceasedToEdit.WardInCharge,
                DeceasedTypeId=deceasedToEdit.DeceasedTypeId,
                 SelectedUsers = (from x in dcx.DeceasedAssignments where x.DeceasedId == @Encryption.Decrypt(Id) select x.UserId).ToList(),

            };
            

            return View(editDeceasedVM);
        }
    }
}
