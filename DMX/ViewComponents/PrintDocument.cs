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

        public IViewComponentResult Invoke(string Id)
        {
               Letter documentToEdit = new Letter();
            documentToEdit = (from d in dcx.Documents where d.DocumentId == Id select d).FirstOrDefault();
            PrintDocumentVM printDocumentVM = new PrintDocumentVM
            {
                AdditionalNotes = documentToEdit.AdditionalNotes,
                Comments = (from c in dcx.Comments where c.TaskId == documentToEdit.DocumentId select c).ToList(),
                ReferenceNumber = documentToEdit.ReferenceNumber,
                //SelectedUsers = AssignedUsers,               
            };
            return View(printDocumentVM);
        }
    }
}
