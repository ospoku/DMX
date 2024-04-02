using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class EditDocument : ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public EditDocument(XContext dContext, UserManager<AppUser>userManager)
        {
            dcx = dContext;
            usm = userManager;
        }
        public IViewComponentResult Invoke(string Id)
        {
            Document documentToEdit = new Document();
            documentToEdit = (from a in dcx.Documents where a.DocumentId == Id & a.IsDeleted == false select a).FirstOrDefault();
            EditDocumentVM editDocumentVM = new EditDocumentVM()
            {
                DocumentDate = documentToEdit.DocumentDate,
                AdditionalNotes=documentToEdit.AdditionalNotes,
                DocumentSource=documentToEdit.DocumentSource,
                ReferenceNumber=documentToEdit.ReferenceNumber,
                DateReceived=documentToEdit.DateReceived,
               
                SelectedUsers = (from x in dcx.Assignments where x.TaskId
                                 == Id
                                 select x.SelectedUsers).FirstOrDefault().Split(','),

                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            return View(editDocumentVM);
        }
    }
}
