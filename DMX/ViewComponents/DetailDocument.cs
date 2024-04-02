using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class DetailDocument : ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public DetailDocument(XContext dContext, UserManager<AppUser> userManager)
        {
            dcx = dContext;
            usm = userManager;
        }
        public IViewComponentResult Invoke(string Id)
        {
             var stringIDs = (from x in dcx.Assignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(',');

            Document documentDetail = new Document();
            documentDetail = (from a in dcx.Documents where a.DocumentId == Id & a.IsDeleted == false select a).FirstOrDefault();
            DetailDocumentVM detailDocumentVM = new DetailDocumentVM()
            {



                DocumentDate = documentDetail.DocumentDate,

                DocumentSource=documentDetail.DocumentSource,
                ReceiptDate=documentDetail.DateReceived,
                ReferenceNumber=documentDetail.ReferenceNumber,
                AdditionalNotes=documentDetail.AdditionalNotes,
                SelectedUsers = (from x in dcx.Assignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(','),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),



            };
            return View(detailDocumentVM);
        }
    }
}
