using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class PrintDocument(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {


        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        
        public IViewComponentResult Invoke(Guid Id)
        {
               Letter documentToEdit = new Letter();
            documentToEdit = (from d in dcx.Letters where d.LetterId == Id select d).FirstOrDefault();
            PrintDocumentVM printDocumentVM = new PrintDocumentVM
            {
                AdditionalNotes = documentToEdit.AdditionalNotes,
                Comments = (from c in dcx.LetterComments where c.LetterId == documentToEdit.LetterId select c).ToList(),
                ReferenceNumber = documentToEdit.ReferenceNumber,
                //SelectedUsers = AssignedUsers,               
            };
            return View(printDocumentVM);
        }
    }
}
