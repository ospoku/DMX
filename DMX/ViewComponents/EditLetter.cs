using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;

namespace DMX.ViewComponents
{
    public class EditLetter(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
       

        public IViewComponentResult Invoke(string Id)
        {
           
         var   documentToEdit = (from a in dcx.Letters where a.LetterId == @Encryption.Decrypt(Id) & a.IsDeleted == false select a).FirstOrDefault();
            EditDocumentVM editDocumentVM = new()
            {
                DocumentDate = documentToEdit.DocumentDate,
                AdditionalNotes=documentToEdit.AdditionalNotes,
                Source=documentToEdit.Source,
                ReferenceNumber=documentToEdit.ReferenceNumber,
                DateReceived=documentToEdit.DateReceived,
                Subject=documentToEdit.Subject,
               
               SelectedUsers = (from x in dcx.LetterAssignments where x.LetterId
                                == Id
                                select x.UserId).ToList(),

                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname)))
            };
            return View(editDocumentVM);
        }
    }
}
